# Global-Outreach

A channel-based language translator bot for Discord that bridges global discussions with intelligent dual-translation support.

## Overview

Global-Outreach is a Discord bot that automatically translates messages based on channel-specific language configurations. Each channel is assigned a target language, and the bot provides dual translations: converting foreign messages to the channel's language AND translating native messages to English. This enables seamless multilingual communication and language learning.

## Features

- **Channel-Based Language Assignment**: Configure specific languages for each Discord channel
- **Automatic Language Detection**: Identifies the source language of incoming messages using OpenAI
- **Dual-Translation System**:
  - Translates foreign language messages to the channel's designated language
  - Translates channel-native messages to English for broader accessibility
- **Smart Language Normalization**: Handles language variations (e.g., "Spanish"/"es"/"spa")
- **Context Preservation**: Maintains conversation flow and meaning across translations
- **Powered by OpenAI**: Uses OpenAI's GPT models for accurate, natural translations

## Technology Stack

- **Language**: C# .NET
- **Discord Library**: [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus)
- **Translation Service**: OpenAI API
- **HTTP Communication**: HttpClient for API requests

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or higher)
- Discord Bot Token ([create one here](https://discord.com/developers/applications))
- OpenAI API Key ([get one here](https://platform.openai.com/api-keys))

## Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/Global-Outreach.git
   cd Global-Outreach
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the bot**

   Create a configuration file or set environment variables with:
   - `DISCORD_BOT_TOKEN`: Your Discord bot token
   - `OPENAI_API_KEY`: Your OpenAI API key

4. **Build the project**
   ```bash
   dotnet build
   ```

5. **Run the bot**
   ```bash
   dotnet run --project The16Oracles.GlobalOutreach/The16Oracles.GlobalOutreach
   ```

## Configuration

The bot requires the following permissions on Discord:
- Read Messages/View Channels
- Send Messages
- Read Message History

Invite the bot to your server with these permissions enabled.

## How It Works

### Channel-Based Translation Flow

1. **Setup Language Channels**: Create Discord channels for different languages (Spanish, Chinese, French, Arabic, etc.)
2. **Configure Channel Mappings**: Map each channel ID to its language in `appsettings.json`
3. **Automatic Dual Translation**:
   - **Foreign → Native**: When a user posts in a different language, the bot translates it to the channel's language
   - **Native → English**: When a user posts in the channel's language, the bot also translates it to English

### Example Scenarios

**Spanish Channel**:
- User posts "Hello" (English) → Bot translates: "Hola" (Spanish)
- User posts "Hola" (Spanish) → Bot translates: "Hello" (English)

**Chinese Channel**:
- User posts "Good morning" (English) → Bot translates: "早上好" (Chinese)
- User posts "早上好" (Chinese) → Bot translates: "Good morning" (English)

This creates a rich multilingual environment where:
- Language learners can see translations of native content
- Non-native speakers can participate in any language
- English speakers can follow all conversations

## Project Structure

```
Global-Outreach/
├── The16Oracles.GlobalOutreach/
│   ├── The16Oracles.GlobalOutreach/     # Main bot project
│   │   ├── Models/
│   │   │   ├── BotConfiguration.cs      # Configuration with language helpers
│   │   │   ├── Language.cs              # Language-to-channel mapping
│   │   │   ├── LanguageConfiguration.cs # Language array container
│   │   │   ├── TranslationRequest.cs    # Translation request model
│   │   │   └── UserLanguageContext.cs   # User language tracking
│   │   ├── Services/
│   │   │   ├── DiscordBotService.cs     # Discord bot with channel-based translation
│   │   │   ├── OpenAITranslationService.cs  # OpenAI API integration
│   │   │   ├── LanguageTrackingService.cs   # User language context tracking
│   │   │   └── I*.cs                    # Service interfaces
│   │   ├── Program.cs                   # Application entry point
│   │   ├── appsettings.json             # Configuration (gitignored)
│   │   ├── appsettings.example.json     # Configuration template
│   │   └── The16Oracles.GlobalOutreach.csproj
│   └── The16Oracles.GlobalOutreach.sln  # Solution file
├── CLAUDE.md                             # AI assistant guidance
├── COPYRIGHT.md                          # License and copyright info
├── .gitignore
└── README.md
```

## Development

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Add Dependencies
```bash
dotnet add package <package-name>
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or suggestions, please open an issue on GitHub.

## Acknowledgments

- Built with [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus)
- Powered by [OpenAI](https://openai.com)
