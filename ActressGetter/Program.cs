using ActressGetter.Dmm;
using ActressGetter.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ActressGetter
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static async Task Main()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
                var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";
                var builder = new ConfigurationBuilder().AddEnvironmentVariables();
                if (isDevelopment) builder.AddUserSecrets<Program>();
                Configuration = builder.Build();
                var apiId = Configuration["DMM:ApiId"];
                var affiliateId = Configuration["DMM:AffiliateId"];
                var connectionString = Configuration["ScrapingConnectionString"];
                var visionSubscriptionKey = Configuration["VisionSubscriptionKey"];
                await DmmAccessor.WriteAsync(apiId, affiliateId, connectionString, visionSubscriptionKey);
#if !DEBUG
                using var resultSqlConnection = new SqlConnection(connectionString);
                SitemapAccessor.Create(resultSqlConnection);
#endif
            }
            catch (Exception ex)
            {
                var slackWebhookURL = Configuration["Slack:WebhookURL"];
                var slack = new SlackAccessor();
                await slack.SendMessageAsync(slackWebhookURL, $"{ex.Message}");
                throw;
            }
        }
    }
}
