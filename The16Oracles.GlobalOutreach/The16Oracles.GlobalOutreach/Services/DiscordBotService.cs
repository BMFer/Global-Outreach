using DSharpPlus;
using DSharpPlus.EventArgs;
using The16Oracles.GlobalOutreach.Models;

namespace The16Oracles.GlobalOutreach.Services
{
    public class DiscordBotService
    {
        private readonly DiscordClient _discordClient;
        private readonly ITranslationService _translationService;
        private readonly ILanguageTrackingService _languageTracker;
        private readonly BotConfiguration _config;

        public DiscordBotService(
            BotConfiguration config,
            ITranslationService translationService,
            ILanguageTrackingService languageTracker)
        {
            _config = config;
            _translationService = translationService;
            _languageTracker = languageTracker;

            var discordConfig = new DiscordConfiguration
            {
                Token = config.DiscordToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                AutoReconnect = true
            };

            _discordClient = new DiscordClient(discordConfig);
            _discordClient.MessageCreated += OnMessageCreated;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("Starting Discord bot...");
            await _discordClient.ConnectAsync();
            Console.WriteLine("Bot is connected and running!");
        }

        public async Task StopAsync()
        {
            Console.WriteLine("Stopping Discord bot...");
            await _discordClient.DisconnectAsync();
            Console.WriteLine("Bot disconnected.");
        }

        private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            // Ignore bot messages to prevent loops
            if (e.Author.IsBot)
                return;

            // Ignore empty messages
            if (string.IsNullOrWhiteSpace(e.Message.Content))
                return;

            try
            {
                // Get the channel ID
                var channelId = e.Channel.Id.ToString();

                // Check if this channel is configured for a specific language
                var channelLanguage = _config.GetLanguageByChannelId(channelId);

                if (string.IsNullOrEmpty(channelLanguage))
                {
                    // Channel not configured, ignore message
                    Console.WriteLine($"[{e.Channel.Name}] Channel not configured for translation. Skipping.");
                    return;
                }

                Console.WriteLine($"[{e.Channel.Name}] Channel language: {channelLanguage}");

                // Detect the language of the message
                var detectedLanguage = await _translationService.DetectLanguageAsync(e.Message.Content);
                Console.WriteLine($"[{e.Author.Username}] Message language detected: {detectedLanguage}");

                // Update user's language context
                _languageTracker.UpdateUserLanguage(e.Author.Id, detectedLanguage);

                // If message is NOT in the channel's designated language, translate it
                if (!IsSameLanguage(detectedLanguage, channelLanguage))
                {
                    var translatedText = await _translationService.TranslateAsync(
                        e.Message.Content,
                        channelLanguage,
                        detectedLanguage);

                    // Post translation to the channel
                    await e.Channel.SendMessageAsync(
                        $"**[Translation to {channelLanguage}]** {e.Author.Mention} said:\n{translatedText}");

                    Console.WriteLine($"Translated from {detectedLanguage} to {channelLanguage}: {translatedText}");
                }
                else if (IsSameLanguage(detectedLanguage, channelLanguage))
                {
                    var translatedText = await _translationService.TranslateAsync(
                         e.Message.Content,
                         "English",
                         channelLanguage);

                    // Post translation to the channel
                    await e.Channel.SendMessageAsync(
                        $"**[Translation to English]** {e.Author.Mention} said:\n{translatedText}");

                    Console.WriteLine($"Translated from {channelLanguage} to English: {translatedText}");
                }
                else
                {
                    Console.WriteLine($"Message already in language foriegn to this channel, no translation available.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                // Optionally log to a file or error tracking service
            }
        }

        private bool IsSameLanguage(string detectedLanguage, string targetLanguage)
        {
            // Normalize language names for comparison
            var normalizedDetected = NormalizeLanguageName(detectedLanguage);
            var normalizedTarget = NormalizeLanguageName(targetLanguage);

            return normalizedDetected.Equals(normalizedTarget, StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizeLanguageName(string language)
        {
            // Handle common variations
            return language.ToLower() switch
            {
                "en" or "eng" => "english",
                "es" or "spa" => "spanish",
                "zh" or "chi" or "cn" => "chinese",
                "fr" or "fra" or "fre" => "french",
                "ar" or "ara" or "arabic" or "saudi arabia" or "saudi-arabia" => "arabic",
                _ => language.ToLower()
            };
        }
    }
}
