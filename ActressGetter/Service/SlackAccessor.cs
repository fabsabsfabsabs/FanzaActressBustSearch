using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ActressGetter.Service
{
    public class SlackAccessor
    {
        private static HttpClient HttpClient;
        private const string SlackWebhookURL = "https://hooks.slack.com/services/";

        public SlackAccessor()
        {
            HttpClient = new HttpClient();
        }

        private class SlackWebhook
        {
            public string text { get; set; } = "";
        }

        public async Task SendMessageAsync(string webhookURL, string text)
        {
            var json = JsonSerializer.Serialize(new SlackWebhook()
            {
                text = text,
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await HttpClient.PostAsync(SlackWebhookURL + webhookURL, content);
        }
    }
}
