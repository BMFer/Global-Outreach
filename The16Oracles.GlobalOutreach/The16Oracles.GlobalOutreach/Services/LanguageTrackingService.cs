using System.Collections.Concurrent;
using The16Oracles.GlobalOutreach.Models;

namespace The16Oracles.GlobalOutreach.Services
{
    public class LanguageTrackingService : ILanguageTrackingService
    {
        private readonly ConcurrentDictionary<ulong, UserLanguageContext> _userContexts = new();

        public void UpdateUserLanguage(ulong userId, string language)
        {
            _userContexts.AddOrUpdate(
                userId,
                new UserLanguageContext
                {
                    UserId = userId,
                    DetectedLanguage = language,
                    LastMessageTime = DateTime.UtcNow
                },
                (key, existing) =>
                {
                    existing.DetectedLanguage = language;
                    existing.LastMessageTime = DateTime.UtcNow;
                    return existing;
                });
        }

        public string? GetUserLanguage(ulong userId)
        {
            if (_userContexts.TryGetValue(userId, out var context))
            {
                // Return language if the context is recent (within last 30 minutes)
                if (DateTime.UtcNow - context.LastMessageTime < TimeSpan.FromMinutes(30))
                {
                    return context.DetectedLanguage;
                }
            }
            return null;
        }

        public bool HasUserContext(ulong userId)
        {
            return _userContexts.ContainsKey(userId);
        }
    }
}
