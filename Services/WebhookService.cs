using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BraveNotificationManager.Services
{
    public class WebhookService
    {
        private readonly IConfiguration _config;
        private readonly string _webhookUrl;
        private readonly ILogger<WebhookService> _logger;

        public WebhookService()
        {
        }

        public WebhookService(IConfiguration configuration, ILogger<WebhookService> logger)
        {
            this._config = configuration;
            this._webhookUrl = _config["Configurations:WebhookURL"];
            this._logger = logger;
        }

        public async Task SendWebhookAsync(string jsonPayload)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(_webhookUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger?.LogInformation("Webhook sent successfully");
                    }
                    else
                    {
                        _logger?.LogError($"Webhook failed with status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError($"Webhook error: {ex.Message}");
                }
            }
        }
    }
}
