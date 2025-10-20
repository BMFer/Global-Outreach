using Microsoft.Extensions.Configuration;
using The16Oracles.GlobalOutreach.Models;
using The16Oracles.GlobalOutreach.Services;

namespace The16Oracles.GlobalOutreach
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Global-Outreach Discord Translation Bot ===");
            Console.WriteLine("Loading configuration...\n");

            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var botConfig = new BotConfiguration
            {
                DiscordToken = configuration["Discord:Token"] ??
                    Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN") ??
                    string.Empty,
                OpenAIApiKey = configuration["OpenAI:ApiKey"] ??
                    Environment.GetEnvironmentVariable("OPENAI_API_KEY") ??
                    string.Empty,
                OpenAIModel = configuration["OpenAI:Model"] ?? "gpt-4o-mini",
                Languages = configuration.GetSection("Language").Get<Language[]>() ?? Array.Empty<Language>()
            };

            // Validate configuration
            if (string.IsNullOrEmpty(botConfig.DiscordToken))
            {
                Console.WriteLine("ERROR: Discord token not found!");
                Console.WriteLine("Please set DISCORD_BOT_TOKEN environment variable or add it to appsettings.json");
                return;
            }

            if (string.IsNullOrEmpty(botConfig.OpenAIApiKey))
            {
                Console.WriteLine("ERROR: OpenAI API key not found!");
                Console.WriteLine("Please set OPENAI_API_KEY environment variable or add it to appsettings.json");
                return;
            }

            Console.WriteLine("Configuration loaded successfully!");
            Console.WriteLine($"OpenAI Model: {botConfig.OpenAIModel}");
            Console.WriteLine($"Configured Languages: {botConfig.Languages.Length}");

            if (botConfig.Languages.Length > 0)
            {
                Console.WriteLine("\nLanguage Channel Mappings:");
                foreach (var lang in botConfig.Languages)
                {
                    Console.WriteLine($"  - {lang.Name}: Channel ID {lang.ChannelId}");
                }
            }
            else
            {
                Console.WriteLine("WARNING: No language channels configured!");
            }
            Console.WriteLine();

            // Initialize services
            var httpClient = new HttpClient();
            var translationService = new OpenAITranslationService(httpClient, botConfig);
            var languageTracker = new LanguageTrackingService();
            var botService = new DiscordBotService(botConfig, translationService, languageTracker);

            // Start the bot
            await botService.StartAsync();

            // Keep the bot running
            Console.WriteLine("\nPress Ctrl+C to stop the bot...");
            await Task.Delay(-1);
        }
    }
}
