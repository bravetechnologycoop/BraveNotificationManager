using AdaptiveCards.Templating;
using BraveNotificationManager.CardActions;
using BraveNotificationManager.Models;
using BraveNotificationManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.TeamsFx.Conversation;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace BraveNotificationManager.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ConversationBot _conversation;
        private readonly AdaptiveCardBuilderFactory _cardBuilderFactory;
        private readonly IConfiguration _config;
        private readonly string api_key;


        public NotificationController(ConversationBot conversation, IConfiguration config)
        {
            this._conversation = conversation;
            this._cardBuilderFactory = new AdaptiveCardBuilderFactory();
            this._config = config;
            this.api_key = _config["Configurations:api_key"];
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] JsonObject request, CancellationToken cancellationToken = default)
        {
            var keyFromRequest = request["key"]?.ToString();

            if (keyFromRequest != api_key)
            {
                return Unauthorized("Invalid API key.");
            }

            var alertType = request["AlertType"]?.ToString() ?? "default";

            var builder = _cardBuilderFactory.GetBuilder(alertType);

            var cardContent = await builder.BuildCardAsync(request, cancellationToken);

            var pageSize = 100;
            string continuationToken = null;
            

            do
            {
                var pagedInstallations = await _conversation.Notification.GetPagedInstallationsAsync(pageSize, continuationToken, cancellationToken);
                continuationToken = pagedInstallations.ContinuationToken;
                var installations = pagedInstallations.Data;

                foreach (var installation in installations)
                {
                    await installation.SendAdaptiveCard(JsonConvert.DeserializeObject(cardContent), cancellationToken);
                }

            } while (!string.IsNullOrEmpty(continuationToken));

           

            return Ok(new { message = "Notification sent successfully" });
        }
    }
}
