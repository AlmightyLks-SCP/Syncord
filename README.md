# Syncord

## Description

Syncord is there to offer you a way of logging specific Events from within your SCP SL Server to your Discord Server.  
To make it short, Syncord supports:  
- Logging Events from multiple SL-Servers to one Discord-Server.
- Logging Events from one SL-Server to multiple Discord-Servers.
- Logging multiple Events into the same channel.

What Syncord doesn't support (yet):
- Logging SL-Server Events from a remote machine. (Bot & Server/s have to be on the same machine.)

---
## Resources used / Versions used

### SyncordBot:  
  - .NET Core       3.1  
  - DSharpPlus      3.2.3
  - Newtonsoft      12.0.3
### SyncordInfo:  
  - .NET Framework  4.7.2
### SyncordPlugin:  
  - .NET Framework  4.7.2
  - Synapse         2.2.0
  - Newtonsoft      12.0.3
  - DSharpPlus      3.2.3
  
---
## Configs

### SyncordBot

The default configs for the Discord Bot looks as following:  

```json
{
    "Prefix": "!",
    "BotToken": "Your Bot Token here",
    "Port": 8000,
    "Guilds": []
}
```
The `Prefix` is, as of yet, not important due to the Bot not having any commands as of this point.  
The `BotToken` shall be your own [Discord Bot Token](https://discord.com/developers/).  
The `Port` shall be the Port which your Discord Bot runs on, as well as the Port that your Bot will be listening for requests on.  
Regarding the `Guilds`... The way a Guild-entry is structured is as following:  

```

{
    "GuildID": YourGuildID,
    "ServerPort": PortOfTheDedicatedServer,
    "DedicatedChannels": {
        EventName: ChannelIDForSpecificLog
    }
}
```
The valid Event Names can be found [here](https://github.com/AlmightyLks/Syncord/blob/main/EventNames.md). I am always open for suggestions!


Which could look something like this in completion:

```json
{
    "Prefix": "!",
    "BotToken": "Not this time",
    "Port": 8000,
    "Guilds": [
        {
            "GuildID": 727996170051518504,
            "ServerPort": 8888,
            "DedicatedChannels": {
                "Player Death": 782245173526134814,
                "Player Join": 782687511428333598,
                "Round Start Spawn": 782726617860014081,
                "Player Leave": 782687511428333598,
                "Console Command": 782730725035474944,
                "Remote Admin Command": 782730725035474944,
                "Player Ban": 783428874720772166
            }
        },
        {
            "GuildID": 727610880816316536,
            "ServerPort": 8888,
            "DedicatedChannels": {
                "Player Death": 727610980237836289
            }
        },
        {
            "GuildID": 727996170051518504,
            "ServerPort": 7777,
            "DedicatedChannels": {
                "Player Death": 782245173526134814,
                "Player Join": 782687511428333598,
                "Player Leave": 782687511428333598
            }
        }
    ]
}
```

That way you can combine multiple Discord-Servers with different SL-Servers, cross-logging events.  

### SyncordPlugin

Each server which you want to be connected to your SyncordBot has to point towards the bot's port.

```yaml
[Syncord]
{
# Port which the Discord-Bot is listening to
discordBotPort: 8000
}
```


---

## How to Install






