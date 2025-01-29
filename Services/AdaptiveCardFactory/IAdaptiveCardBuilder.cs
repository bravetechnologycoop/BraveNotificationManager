using System.Text.Json.Nodes;

public interface IAdaptiveCardBuilder
{
    Task<string> BuildCardAsync(JsonObject request, CancellationToken cancellationToken = default);
}
