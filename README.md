# Global-Outreach

A two-way language translator bot for Discord channels that bridges global discussions without breaking context.

## Overview

Global-Outreach listens for messages in Discord channels, automatically identifies the original language, translates them to English, and re-translates English responses back into the local language. This enables seamless multilingual conversations where participants can communicate in their native languages.

## Features

- **Automatic Language Detection**: Identifies the source language of incoming messages
- **Two-Way Translation**:
  - Translates foreign language messages to English for the channel
  - Translates English responses back to the original language
- **Context Preservation**: Maintains conversation flow and meaning across translations
- **Powered by OpenAI**: Uses OpenAI's translation API for accurate, natural translations

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

1. A user posts a message in their native language (e.g., Spanish)
2. The bot detects the message and identifies the language
3. The message is translated to English and posted in the channel
4. When English speakers respond, the bot translates their replies back to Spanish
5. The original user receives the translation in their language

This creates a seamless conversation where everyone can communicate in their preferred language.

## Project Structure

```
Global-Outreach/
├── The16Oracles.GlobalOutreach/
│   ├── The16Oracles.GlobalOutreach/     # Main bot project
│   │   ├── Class1.cs                     # Entry point
│   │   └── The16Oracles.GlobalOutreach.csproj
│   └── The16Oracles.GlobalOutreach.sln  # Solution file
├── CLAUDE.md                             # AI assistant guidance
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
