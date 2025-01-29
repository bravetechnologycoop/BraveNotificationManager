
using BraveNotificationManager.Models;
using AdaptiveCards.Templating;
using Microsoft.Bot.Builder;
using Microsoft.TeamsFx.Conversation;
using Newtonsoft.Json;
using BraveNotificationManager.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace BraveNotificationManager.CardActions
{
    public class HandleAlertResponse : IAdaptiveCardActionHandler
    {
        private readonly string _responseCardFilePath = Path.Combine(".", "Resources", "AlertResponded.json");
        private readonly WebhookService _webhookService;

        public string TriggerVerb => "alertResponded";
        public AdaptiveCardResponse AdaptiveCardResponse => AdaptiveCardResponse.ReplaceForInteractor;

        public HandleAlertResponse(WebhookService webhookService)
        {
            _webhookService = webhookService ?? throw new ArgumentNullException(nameof(webhookService));
        }

        public async Task<InvokeResponse> HandleActionInvokedAsync(ITurnContext turnContext, object cardData, CancellationToken cancellationToken = default)
        {
            var eventID = (string)((dynamic)cardData).eventID;
            var userName = turnContext.Activity.From.Name;

            var cardTemplate = await File.ReadAllTextAsync(_responseCardFilePath, cancellationToken);
            var cardContent = new AdaptiveCardTemplate(cardTemplate).Expand(new AlertRespondedModel
            {
                Title = "Alert Responded.",
                Description = $"{userName} has responded to this alert",
            });


            return InvokeResponseFactory.AdaptiveCard(JsonConvert.DeserializeObject(cardContent));
        }

        public async Task<InvokeResponse> HandleActionInvokedAsync()
        {
            //var eventID = (string)((dynamic)cardData).eventID;
            //var userName = turnContext.Activity.From.Name;

            var cardTemplate = await File.ReadAllTextAsync(_responseCardFilePath);
            var cardContent = new AdaptiveCardTemplate(cardTemplate).Expand(new AlertRespondedModel
            {
                Title = "Alert Responded.",
                Description = " has responded to this alert",
            });


            return InvokeResponseFactory.AdaptiveCard(JsonConvert.DeserializeObject(cardContent));
        }
    }
}
