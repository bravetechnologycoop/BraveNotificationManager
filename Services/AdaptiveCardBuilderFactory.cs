public class AdaptiveCardBuilderFactory
{
    private readonly Dictionary<string, IAdaptiveCardBuilder> _builders;

    public AdaptiveCardBuilderFactory()
    {
        _builders = new Dictionary<string, IAdaptiveCardBuilder>
        {
            { "stillness", new StillnessAlertBuilder() },
            { "survey", new SurveyBuilder() },
            { "lowbattery", new NotificationResponselessBuilder() },
            { "disconnection", new NotificationResponselessBuilder() }
        };
    }

    public IAdaptiveCardBuilder GetBuilder(string alertType)
    {
        return _builders.GetValueOrDefault(alertType.ToLower(), new StillnessAlertBuilder());
    }
}
