namespace The16Oracles.GlobalOutreach.Services
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = "auto");
        Task<string> DetectLanguageAsync(string text);
    }
}
