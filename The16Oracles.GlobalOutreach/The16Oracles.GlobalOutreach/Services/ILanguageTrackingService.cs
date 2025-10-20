using The16Oracles.GlobalOutreach.Models;

namespace The16Oracles.GlobalOutreach.Services
{
    public interface ILanguageTrackingService
    {
        void UpdateUserLanguage(ulong userId, string language);
        string? GetUserLanguage(ulong userId);
        bool HasUserContext(ulong userId);
    }
}
