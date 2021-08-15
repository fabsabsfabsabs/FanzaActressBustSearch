using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ActressGetter.Service
{
    internal class VisionAccessor
    {
        private static readonly string AnalyzeUrl = "https://fanzaactressbustsearch.cognitiveservices.azure.com/vision/v2.0/analyze?visualFeatures=description,adult";
        private static HttpClient HttpClient;
        private string VisionSubscriptionKey;

        internal VisionAccessor(string visionSubscriptionKey)
        {
            VisionSubscriptionKey = visionSubscriptionKey;
            ReBuild();
        }

        internal void ReBuild()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", VisionSubscriptionKey);
        }

        internal  async Task<AnalyzeImage> GetAnalyzeImageAsync(string imageUrl)
        {
            var getResponseMessage = await HttpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
            var postResponseMessage = await HttpClient.PostAsync(AnalyzeUrl, getResponseMessage.Content);
            var contentString = await postResponseMessage.Content.ReadAsStringAsync();
            if (!contentString.Contains("isAdultContent"))
            {
                Console.WriteLine("Error:contentString:" + contentString);
                return new AnalyzeImage();
            }

            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(contentString));
            var image = new DataContractJsonSerializer(typeof(AnalyzeImage)).ReadObject(ms) as AnalyzeImage;
            if (image.adult.adultScore == 0)
            {
                Console.WriteLine("Error:getResponseMessage:" + getResponseMessage.RequestMessage);
                Console.WriteLine("Error:postResponseMessage:" + postResponseMessage.RequestMessage);
                ReBuild();
            }
            return image;
        }
    }
}
