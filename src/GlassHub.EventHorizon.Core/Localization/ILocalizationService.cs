namespace GlassHub.EventHorizon.Core.Localization;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    void SetCulture(string cultureCode);
    string GetString(string key);
    string Format(string key, params object[] args);
}
