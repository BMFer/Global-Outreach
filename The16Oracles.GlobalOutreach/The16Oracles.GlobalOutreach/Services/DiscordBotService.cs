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
                // Detect the language of the message
                var detectedLanguage = await _translationService.DetectLanguageAsync(e.Message.Content);
                Console.WriteLine($"[{e.Author.Username}] Language detected: {detectedLanguage}");

                // Update user's language context
                _languageTracker.UpdateUserLanguage(e.Author.Id, detectedLanguage);

                // If message is not in English, translate to English
                if (!IsEnglish(detectedLanguage))
                {
                    var translatedText = await _translationService.TranslateAsync(
                        e.Message.Content,
                        "English",
                        detectedLanguage);

                    // Post translation to the channel
                    await e.Channel.SendMessageAsync(
                        $"**[Translation from {detectedLanguage}]** {e.Author.Mention} said:\n{translatedText}");

                    Console.WriteLine($"Translated to English: {translatedText}");
                }
                // If message is in English, check if we should translate back to other languages
                else
                {
                    // Check if this is a reply or in a conversation with non-English speakers
                    await HandleEnglishMessageAsync(e);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                // Optionally log to a file or error tracking service
            }
        }

        private async Task HandleEnglishMessageAsync(MessageCreateEventArgs e)
        {
            // Get recent messages from the channel to determine if this is a response
            var recentMessages = await e.Channel.GetMessagesAsync(10);

            // Find recent non-English speakers
            var recentNonEnglishUsers = recentMessages
                .Where(m => !m.Author.IsBot && m.Author.Id != e.Author.Id)
                .Select(m => m.Author.Id)
                .Distinct()
                .Where(userId => _languageTracker.HasUserContext(userId))
                .ToList();

            // If there are recent non-English speakers, translate the English message back
            foreach (var userId in recentNonEnglishUsers)
            {
                var targetLanguage = _languageTracker.GetUserLanguage(userId);
                if (targetLanguage != null && !IsEnglish(targetLanguage))
                {
                    var user = recentMessages.First(m => m.Author.Id == userId).Author;
                    var translatedText = await _translationService.TranslateAsync(
                        e.Message.Content,
                        targetLanguage,
                        "English");

                    await e.Channel.SendMessageAsync(
                        $"**[Translation to {targetLanguage}]** {user.Mention}:\n{translatedText}");

                    Console.WriteLine($"Translated English message to {targetLanguage}: {translatedText}");

                    // Only translate for the most recent non-English speaker to avoid spam
                    break;
                }
            }
        }

        private bool IsEnglish(string language)
        {
            return language.Equals("English", StringComparison.OrdinalIgnoreCase) ||
                   language.Equals("en", StringComparison.OrdinalIgnoreCase);
        }
    }
}
