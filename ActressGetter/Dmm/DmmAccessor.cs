using ActressGetter.SqlServer;
using ActressGetter.Service;
using Newtonsoft.Json;
using FanzaActressSearch.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Z.Dapper.Plus;
using Dapper;
using System.Transactions;
using System.Threading;

namespace ActressGetter.Dmm
{
    internal static class DmmAccessor
    {
        private static readonly string DmmApiUrl = "https://api.dmm.com/affiliate/v3/";
        private static readonly int Hits = 100;
        private static HttpClient HttpClient { get; set; } = new HttpClient();
        private static VisionAccessor VisionAccessor { get; set; }
        private static WikipediaAccessor WikipediaAccessor { get; set; }
        
        internal static async Task WriteAsync(string apiId, string affiliateId, string connectionString, string visionSubscriptionKey, string actressId = "")
        {
            VisionAccessor = new VisionAccessor(visionSubscriptionKey);
            WikipediaAccessor = new WikipediaAccessor();

            var actressList = (await GetScrapingActressListAsync(apiId, affiliateId, actressId)).ToList();
            for (int i = 0; i < actressList.Count; i++)
            {
                var actress = actressList[i];
                Console.WriteLine($"{actress.Id}:{actress.Name}:{i + 1}/{actressList.Count}");
                var productList = (await actress.GetProductListAsync(apiId, affiliateId)).ToList();
                if (productList.Any())
                {
                    var max = productList.Max(x => x.Date);
                    var min = productList.Min(x => x.Date);
                    actress.FirstSingle = max != min ? min : new DateTime(1900, 1, 1);
                    actress.LastSingle = max != min ? max : new DateTime(1900, 1, 1);
                }
                else
                {
                    actress.FirstSingle = new DateTime(1900, 1, 1);
                    actress.LastSingle = new DateTime(1900, 1, 1);
                }
                Actress preActress;
                {
                    using var sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    preActress = sqlConnection.SingleOrDefaultActress(actress.Id);
                }
                //女優　最新作品が前回より新しい場合のみ更新
                var updateActress = preActress == null || preActress?.LastSingle < actress.LastSingle;

                if (actress.ImageURL != "")
                {
                    try
                    {
                        if (preActress?.ImageURL != actress.ImageURL)
                        {
                            Console.WriteLine($"{actress.Id}:{actress.Name}:Vision解析");
                            var image = await VisionAccessor.GetAnalyzeImageAsync(actress.GetImageUrl(true));
                            actress.IsAdultImage = image.adult.isAdultContent;
                        }
                        else
                        {
                            actress.IsAdultImage = preActress.IsAdultImage;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error Vision : {ex.Message}\n{ex.StackTrace}");
                        continue;
                    }
                }

                try
                {
                    if (updateActress)
                    {
                        Console.WriteLine($"{actress.Id}:{actress.Name}:Wikipedia解析");

                        actress.Wiki = await WikipediaAccessor.GetExtractTextAsync(actress.Name);
                        if (actress.Wiki != "")
                        {
                            (actress.TwitterName, actress.InstagramName) = await WikipediaAccessor.GetTwitterAndInstagramNameAsync(actress.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Wikipedia : {ex.Message}\n{ex.StackTrace}");
                    continue;
                }


                Console.WriteLine($"{actress.Id}:{actress.Name}:SQL書込");
                try
                {
                    using var sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();

                    //女優　最新作品が前回より新しい場合のみ更新
                    if (updateActress)
                    {
                        sqlConnection.InsertOrUpdateActress(actress, DateTime.Now);
                    }
                    //商品
                    var preProductList = sqlConnection.SelectProduct(actress.Id);
                    //新規
                    foreach (var product in productList.Except(preProductList, new ProductComparer()))
                    {
                        sqlConnection.InsertOrUpdateProduct(product, actress.Id, DateTime.Now);
                    }
                    //削除
                    foreach (var product in preProductList.Except(productList, new ProductComparer()))
                    {
                        sqlConnection.DeleteProduct(product);
                    }
                    //発売前はなるべく更新
                    foreach(var product in productList.Where(x => x.Date > DateTime.Now))
                    {
                        sqlConnection.InsertOrUpdateProduct(product, actress.Id, DateTime.Now);
                    }

                    Console.WriteLine($"actressId:{actressId}:SQL更新");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error SQL : {ex.Message}\n{ex.StackTrace}");
                    continue;
                }
            }
            using var resultSqlConnection = new SqlConnection(connectionString);
            resultSqlConnection.Open();
            resultSqlConnection.Insert(new ActressScrapingResult()
            {
                InsertCount = actressList.Count,
                CreateDate = DateTime.Now,
            });
        }

        internal static async Task ReBuildActressProductAsync(string apiId, string affiliateId, string connectionString)
        {
            var list = (await GetActressProductsAsync(apiId, affiliateId)).ToList();
            using var ts = new TransactionScope();
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            sqlConnection.Execute("TRUNCATE TABLE ActressProduct");
            sqlConnection.BulkInsert(list);
            ts.Complete();
        }

        private static async Task<IEnumerable<ActressProduct>> GetActressProductsAsync(string apiId, string affiliateId)
        {
            var actressProductList = new List<ActressProduct>();
            var actressList = (await GetScrapingActressListAsync(apiId, affiliateId, "")).ToList();
            for (int i = 0; i < actressList.Count; i++)
            {
                var actress = actressList[i];

                Console.WriteLine($"{actress.Id}:{actress.Name}:{i + 1}/{actressList.Count}");
                actressProductList.AddRange((await actress.GetProductListAsync(apiId, affiliateId)).Select(p => new ActressProduct() { ActressID = actress.Id, ProductID = p.Id }));
            }
            return actressProductList;
        }

        internal static async Task<IEnumerable<Actress>> GetScrapingActressAsync(string apiId, string affiliateId, string actressId)
        {
            var response = await HttpClient.GetAsync($"{DmmApiUrl}/ActressSearch?api_id={apiId}&affiliate_id={affiliateId}&actress_id={actressId}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var actressSearch = JsonConvert.DeserializeObject<ActressSearchJson>(jsonString);
            return actressSearch.ToActressList();
        }

        internal static async Task<IEnumerable<Actress>> GetScrapingActressListAsync(string apiId, string affiliateId, string actressId)
        {
            int offset = 1;
            var actressList = new List<Actress>();
            while(true)
            {
                HttpResponseMessage response;
                if (string.IsNullOrWhiteSpace(actressId))
                {
                    response = await HttpClient.GetAsync($"{DmmApiUrl}/ActressSearch?api_id={apiId}&affiliate_id={affiliateId}&hits={Hits}&offset={offset}");
                }
                else
                {
                    response = await HttpClient.GetAsync($"{DmmApiUrl}/ActressSearch?api_id={apiId}&affiliate_id={affiliateId}&actress_id={actressId}");
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var actressSearch = JsonConvert.DeserializeObject<ActressSearchJson>(jsonString);
                if (actressSearch.result.result_count == 0) break;
                if (actressSearch.result.status != "200") continue;

                actressList.AddRange(actressSearch.ToActressList());
                offset += Hits;
                Console.WriteLine($"{actressList.Count}人取得！");
                Thread.Sleep(1000);

                if (!string.IsNullOrWhiteSpace(actressId)) break;
            }
            return actressList.Distinct();
        }

        private static async Task<IEnumerable<Product>> GetProductListAsync(this Actress actress, string apiId, string affiliateId)
        {
            int offset = 1;
            var productList = new List<Product>();
            do
            {
                string jsonString;
                try
                {
                    var response = await HttpClient.GetAsync($"{DmmApiUrl}/ItemList?api_id={apiId}&affiliate_id={affiliateId}&site=FANZA&service=digital&article=actress&article_id={actress.Id}&hits={Hits}&offset={offset}");
                    jsonString = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error http : {ex.Message}\n{ex.StackTrace}");
                    continue;
                }
                var productSearchJson = JsonConvert.DeserializeObject<ProductSearchJson>(jsonString);
                if (productSearchJson.result.result_count == 0 || productSearchJson.result.status != 200) break;

                productList.AddRange(productSearchJson.ToProductList());
                Console.WriteLine($"{actress.Id}:{actress.Name}:{productList.Count}作品取得！");
                if (offset == 1) actress.WorkCounter = productSearchJson.result.total_count;
                offset += Hits;
            }
            while (offset < actress.WorkCounter);
            return productList.Distinct();
        }
    }
}
