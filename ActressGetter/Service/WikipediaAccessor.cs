using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ActressGetter.Service
{
    public class WikipediaAccessor
    {
        private static HttpClient HttpClient;
        private const string WikipediaApiExtractsUrl = "https://ja.wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&exlimit=max&explaintext&exintro&&titles=";
        private const string WikipediaApiRevisionsUrl = "https://ja.wikipedia.org/w/api.php?format=xml&action=query&prop=revisions&rvprop=content&titles=";

        public WikipediaAccessor()
        {
            HttpClient = new HttpClient();
        }

        public void ReBuild()
        {
            HttpClient = new HttpClient();
        }

        private static async Task<string> ReadAsStringAsync(string url)
        {
            var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStringAsync();
        }

        //リダイレクト対応
        private static async Task<string> GetRedirectName(string title)
        {
            var revisionText = await ReadAsStringAsync($"{WikipediaApiRevisionsUrl}{HttpUtility.UrlEncode(title)}");
            if (revisionText.ToUpper().Contains("#REDIRECT"))
            {
                if (revisionText.Contains("[[Special:ApiFeatureUsage]]"))
                {
                    revisionText = revisionText.Substring(revisionText.IndexOf("[[Special:ApiFeatureUsage]]") + "[[Special:ApiFeatureUsage]]".Length);
                }
                title = Pullout(revisionText, "[[", "]]");
            }
            return title;
        }

        private static string Pullout(string value, string before, string after)
        {
            var bIndex = value.IndexOf(before);
            var aIndex = value.IndexOf(after);
            if (bIndex == -1 || aIndex == -1) return "";
            return value.Substring(bIndex + before.Length, aIndex - bIndex - before.Length);
        }

        public static async Task<string> GetExtractTextAsync(string title)
        {
            title = await GetRedirectName(title);

            var wikiText = await ReadAsStringAsync($"{WikipediaApiExtractsUrl}{HttpUtility.UrlEncode(title)}");
            var text = Regex.Replace(wikiText, "<[^>]+>", "");
            return text.Contains("女優") ? text : "";
        }

        public static async Task<(string, string)> GetTwitterAndInstagramNameAsync(string title)
        {
            title = await GetRedirectName(title);

            var wikiText = await ReadAsStringAsync($"{WikipediaApiRevisionsUrl}{HttpUtility.UrlEncode(title)}");
            var tmatch = new Regex(@"(Twitter|twitter)\|[a-zA-Z0-9_]+", RegexOptions.IgnoreCase).Matches(wikiText);
            var twitterName = tmatch.Any() ? tmatch.Last().Value.Substring(tmatch.Last().Value.IndexOf('|') + 1) : "";
            var imatch = new Regex(@"(Instagram)\|[a-zA-Z0-9_]+", RegexOptions.IgnoreCase).Matches(wikiText);
            var instagramName = imatch.Any() ? imatch.Last().Value.Substring(imatch.Last().Value.IndexOf('|') + 1) : "";
            return (twitterName, instagramName);
        }
    }
}
