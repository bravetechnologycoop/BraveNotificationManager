using AdaptiveCards.Templating;
using BraveNotificationManager.Models;
using System.Text.Json.Nodes;

public class StillnessAlertBuilder : IAdaptiveCardBuilder
{
    private readonly string _cardTemplatePath = Path.Combine(".", "Resources", "StillnessAlert.json");

    public async Task<string> BuildCardAsync(JsonObject request, CancellationToken cancellationToken = default)
    {
        var cardTemplate = await System.IO.File.ReadAllTextAsync(_cardTemplatePath, cancellationToken);

        var model = new StillnessAlertModel
        {
            AlertType = request["AlertType"]?.ToString(),
            SiteData = request["SiteData"]?.ToString(),
            EventID = request["EventId"]?.ToString()
        };

        return new AdaptiveCardTemplate(cardTemplate).Expand(model);
    }
}
