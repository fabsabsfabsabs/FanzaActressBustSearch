using Dapper;
using FanzaActressSearch.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using XSitemaps;

namespace ActressGetter.Service
{
    public static class SitemapAccessor
    {
        private static readonly string TopPageUrl = "https://fanza-actress-bust-search.azurewebsites.net";
        public static void Create(this SqlConnection sqlConnection)
        {
            var options = new SerializeOptions
            {
                EnableIndent = true,
                EnableGzipCompression = false,
            };
            var sitemapUrlList = GetSitemapUrlList(sqlConnection);
            var sitemaps = Sitemap.Create(sitemapUrlList.ToArray());
            var modifiedAt = DateTimeOffset.Now;
            var siteMapInfoList = new List<SitemapInfo>();
            var indexMax = sitemapUrlList.Count / 50000 + 1;
            for (var i = 0; i < indexMax; i++)
            {
                using var stream = new MemoryStream();
                sitemaps[i].Serialize(stream, options);
                File.WriteAllText(@$"C:\home\site\wwwroot\wwwroot\sitemap{i}.xml", Encoding.UTF8.GetString(stream.ToArray()));

                siteMapInfoList.Add(new SitemapInfo($"{TopPageUrl}/sitemap{i}.xml", modifiedAt));
            }
            using var indexStream = new MemoryStream();
            new SitemapIndex(siteMapInfoList).Serialize(indexStream, options);
            var contents = Encoding.UTF8.GetString(indexStream.ToArray());
            File.WriteAllText(@"C:\home\site\wwwroot\wwwroot\sitemapIndex.xml", contents);
        }

        private static List<SitemapUrl> GetSitemapUrlList(this SqlConnection sqlConnection)
        {
            var date = sqlConnection.Query<DateTime>($"select Top 1 CreateDate from ActressScrapingResult order by CreateDate desc").SingleOrDefault();
            var actress = sqlConnection.Query<Actress>($"select Id, UpdateDate from Actress");
            var product = sqlConnection.Query(@"select ActressProduct.ActressID, Product.ProductID, Product.UpdateDate from ActressProduct 
                                                inner join Actress on ActressProduct.ActressID = Actress.Id
                                                inner join Product on ActressProduct.ProductID = Product.Id");
            var urls = new List<SitemapUrl>()
            {
                new SitemapUrl($"{TopPageUrl}", date, ChangeFrequency.Daily),
            };
            urls.AddRange(actress.Select(a => new SitemapUrl($"{TopPageUrl}/actress/{a.Id}", a.UpdateDate, ChangeFrequency.Weekly, 0.5)));
            urls.AddRange(product.Select(p => new SitemapUrl($"{TopPageUrl}/actress/{p.ActressID}/product/{p.ProductID}", p.UpdateDate, ChangeFrequency.Weekly, 0.5)));
            return urls;
        }
    }
}
