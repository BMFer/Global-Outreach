# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Global-Outreach is a two-way language translator bot for Discord channels. It enables seamless global communication by:
- Listening for messages in any language on Discord
- Identifying the original language automatically
- Translating messages to English for the channel
- Re-translating English responses back into the original language
- Maintaining conversation context across language barriers

## Technology Stack

- **Language**: C# .NET
- **Discord Library**: DSharpPlus
- **Translation Service**: OpenAI API (via HttpClient)
- **Communication**: HTTP-based API calls to OpenAI

## Common Commands

### Build and Run
```bash
dotnet build                    # Build the solution
dotnet run                      # Run the bot
dotnet run --project <path>     # Run specific project
```

### Testing
```bash
dotnet test                     # Run all tests
dotnet test --filter <filter>   # Run specific tests
```

### Package Management
```bash
dotnet restore                  # Restore NuGet packages
dotnet add package <name>       # Add a new package
```

## Architecture

### Core Components

**Discord Event Handling**
- Uses DSharpPlus to connect to Discord and listen for message events
- Event handlers capture messages from channels and process them asynchronously

**Language Detection**
- Identifies the source language of incoming messages
- Maintains language context per user/channel to enable proper back-translation

**Translation Pipeline**
- **Inbound**: Original language → English (for channel visibility)
- **Outbound**: English responses → Original language (for original sender)
- Uses OpenAI API via HttpClient for translation requests
- Preserves message context and conversation flow

**API Integration**
- HttpClient-based communication with OpenAI
- Handles API authentication, rate limiting, and error responses
- Manages asynchronous translation requests

### Message Flow

1. User posts message in their native language
2. Bot detects message event via DSharpPlus
3. Language identification determines source language
4. Message is translated to English and posted
5. English responses are captured
6. Bot translates responses back to original language
7. Translated response is delivered to original user

## Configuration

The bot requires:
- Discord bot token for DSharpPlus authentication
- OpenAI API key for translation services
- Channel permissions for reading and posting messages

## Key Design Considerations

- **Stateful Language Tracking**: The bot must track which language each user is communicating in to enable proper back-translation
- **Context Preservation**: Translations should maintain the meaning and tone of the original message
- **Asynchronous Operations**: All Discord events and API calls should be handled asynchronously to avoid blocking
- **Error Handling**: Gracefully handle API failures, rate limits, and network issues without disrupting the conversation flow
