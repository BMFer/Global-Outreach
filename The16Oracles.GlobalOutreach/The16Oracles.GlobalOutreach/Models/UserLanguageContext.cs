namespace The16Oracles.GlobalOutreach.Models
{
    public class UserLanguageContext
    {
        public ulong UserId { get; set; }
        public string DetectedLanguage { get; set; } = string.Empty;
        public DateTime LastMessageTime { get; set; }
    }
}
