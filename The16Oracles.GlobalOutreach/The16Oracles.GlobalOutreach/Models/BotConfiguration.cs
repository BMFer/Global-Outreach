namespace The16Oracles.GlobalOutreach.Models
{
    public class BotConfiguration
    {
        public string DiscordToken { get; set; } = string.Empty;
        public string OpenAIApiKey { get; set; } = string.Empty;
        public string OpenAIModel { get; set; } = "gpt-4o-mini";
        public Language[] Languages { get; set; } = Array.Empty<Language>();

        /// <summary>
        /// Get the language name for a given channel ID
        /// </summary>
        public string? GetLanguageByChannelId(string channelId)
        {
            return Languages.FirstOrDefault(l => l.ChannelId == channelId)?.Name;
        }

        /// <summary>
        /// Get the channel ID for a given language name
        /// </summary>
        public string? GetChannelIdByLanguage(string languageName)
        {
            return Languages.FirstOrDefault(l => l.Name.Equals(languageName, StringComparison.OrdinalIgnoreCase))?.ChannelId;
        }

        /// <summary>
        /// Check if a channel ID is configured for any language
        /// </summary>
        public bool IsLanguageChannel(string channelId)
        {
            return Languages.Any(l => l.ChannelId == channelId);
        }
    }
}
