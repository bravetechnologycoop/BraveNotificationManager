using AdaptiveCards.Templating;
using BraveNotificationManager.Models;
using System.Text.Json.Nodes;

public class NotificationResponselessBuilder : IAdaptiveCardBuilder
{
    private readonly string _cardTemplatePath = Path.Combine(".", "Resources", "NotificationResponseless.json");

    public async Task<string> BuildCardAsync(JsonObject request, CancellationToken cancellationToken = default)
    {
        var cardTemplate = await System.IO.File.ReadAllTextAsync(_cardTemplatePath, cancellationToken);

        var model = new NotificationResponselessModel
        {
            AlertType = request["AlertType"]?.ToString(),
            Description = request["Description"]?.ToString()
        };

        return new AdaptiveCardTemplate(cardTemplate).Expand(model);
    }
}
