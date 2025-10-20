# Global-Outreach Discord Translation Bot

## Setup Instructions

### 1. Configure API Keys

You have two options to configure your Discord and OpenAI API keys:

#### Option A: Using appsettings.json (Recommended)

1. Copy `appsettings.example.json` to `appsettings.json`:
   ```bash
   cp appsettings.example.json appsettings.json
   ```

2. Edit `appsettings.json` and add your API keys:
   ```json
   {
     "Discord": {
       "Token": "YOUR_DISCORD_BOT_TOKEN_HERE"
     },
     "OpenAI": {
       "ApiKey": "YOUR_OPENAI_API_KEY_HERE",
       "Model": "gpt-4o-mini"
     }
   }
   ```

#### Option B: Using Environment Variables

Set the following environment variables:

**Windows (PowerShell):**
```powershell
$env:DISCORD_BOT_TOKEN="your_discord_token"
$env:OPENAI_API_KEY="your_openai_key"
```

**Windows (Command Prompt):**
```cmd
set DISCORD_BOT_TOKEN=your_discord_token
set OPENAI_API_KEY=your_openai_key
```

**Linux/macOS:**
```bash
export DISCORD_BOT_TOKEN="your_discord_token"
export OPENAI_API_KEY="your_openai_key"
```

### 2. Get Your API Keys

#### Discord Bot Token
1. Go to [Discord Developer Portal](https://discord.com/developers/applications)
2. Create a new application or select an existing one
3. Go to the "Bot" section
4. Click "Reset Token" and copy your bot token
5. Enable the following Privileged Gateway Intents:
   - Message Content Intent

#### OpenAI API Key
1. Go to [OpenAI API Keys](https://platform.openai.com/api-keys)
2. Create a new API key
3. Copy and save it securely

### 3. Invite Bot to Your Discord Server

1. In the Discord Developer Portal, go to "OAuth2" → "URL Generator"
2. Select the following scopes:
   - `bot`
3. Select the following bot permissions:
   - Read Messages/View Channels
   - Send Messages
   - Read Message History
4. Copy the generated URL and open it in your browser
5. Select your server and authorize the bot

### 4. Run the Bot

```bash
dotnet run
```

The bot will start and connect to Discord. You should see:
```
=== Global-Outreach Discord Translation Bot ===
Loading configuration...

Configuration loaded successfully!
OpenAI Model: gpt-4o-mini

Starting Discord bot...
Bot is connected and running!

Press Ctrl+C to stop the bot...
```

## How It Works

The bot uses **channel-based language configuration** with **dual-translation support**. Each channel is assigned a specific language, and the bot provides translations to help bridge language barriers.

### Translation Flow:

1. **Configure Language Channels**: Set up Discord channels for each language (Spanish, Chinese, French, etc.)
2. **Map Channels in Config**: Add the channel IDs to your `appsettings.json` under the `Language` array
3. **Dual-Translation System**:
   - **Foreign Language → Channel Language**: If someone posts in a different language, it's translated to the channel's language
   - **Channel Language → English**: If someone posts in the channel's native language, it's also translated to English for broader understanding

### Translation Scenarios:

#### Scenario 1: Foreign Language Posted in Channel
**Spanish Channel** - User posts "Hello" (English):
- ✅ Bot translates to Spanish: "Hola"
- User posts "你好" (Chinese):
- ✅ Bot translates to Spanish: "Hola"

#### Scenario 2: Native Language Posted in Channel
**Spanish Channel** - User posts "Hola" (Spanish):
- ✅ Bot translates to English: "Hello"

**Chinese Channel** - User posts "早上好" (Chinese):
- ✅ Bot translates to English: "Good morning"

#### Scenario 3: English Channel
**English Channel** - User posts "Hello" (English):
- ✅ Bot translates to English: "Hello" (for consistency)

**English Channel** - User posts "Bonjour" (French):
- ✅ Bot translates to English: "Hello"

### Key Benefits:

- **Language Learners**: Native speakers can post in their language and see the English translation
- **Cross-Language Communication**: Anyone can post in any language and it gets translated to the channel's language
- **Global Accessibility**: English translations make all conversations accessible to English speakers

## Project Structure

```
The16Oracles.GlobalOutreach/
├── Models/
│   ├── BotConfiguration.cs          # Configuration settings with language helpers
│   ├── Language.cs                  # Language model (Name, ChannelId)
│   ├── LanguageConfiguration.cs     # Language array container
│   ├── TranslationRequest.cs        # Translation request model
│   └── UserLanguageContext.cs       # User language tracking
├── Services/
│   ├── ITranslationService.cs       # Translation service interface
│   ├── OpenAITranslationService.cs  # OpenAI implementation
│   ├── ILanguageTrackingService.cs  # Language tracking interface
│   ├── LanguageTrackingService.cs   # Language tracking implementation
│   └── DiscordBotService.cs         # Discord bot logic
├── Program.cs                        # Application entry point
├── appsettings.example.json         # Configuration template
└── appsettings.json                 # Your actual config (git ignored)
```

## Troubleshooting

### Bot doesn't respond to messages
- Ensure "Message Content Intent" is enabled in Discord Developer Portal
- Check that the bot has permissions in the channel
- Verify your API keys are correct

### OpenAI API errors
- Check your OpenAI API key is valid
- Ensure you have credits in your OpenAI account
- Verify the model name is correct (default: gpt-4o-mini)

### Build errors
- Ensure you have .NET 8.0 SDK installed
- Run `dotnet restore` to restore packages
- Run `dotnet clean` followed by `dotnet build`
