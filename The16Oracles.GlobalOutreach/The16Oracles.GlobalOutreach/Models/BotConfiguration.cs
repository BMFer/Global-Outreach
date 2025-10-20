namespace The16Oracles.GlobalOutreach.Models
{
    public class BotConfiguration
    {
        public string DiscordToken { get; set; } = string.Empty;
        public string OpenAIApiKey { get; set; } = string.Empty;
        public string OpenAIModel { get; set; } = "gpt-4o-mini";
    }
}
