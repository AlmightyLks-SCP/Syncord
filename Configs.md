## Configs

### SyncordBot

The default configs for the Discord Bot looks as following:  

```json
{
  "Prefix": "!",
  "BotToken": "Your Bot Token here",
  "Port": 8000,
  "DiscordActivity": {
    "Name": "{SLCount} Server/s",
    "Activity": 3
  },
  "Guilds": [
  ],
  "EmbedConfigs": {
    "PlayerJoinedLeftConfig": {
      "ShowUserId": true,
      "ShowPing": true,
      "ShowIP": false
    },
    "PlayerDeathConfig": {
      "ShowUserId": true
    },
    "RoundEndConfig": {
      "ShowRoundLength": true,
      "ShowTotalKills": true,
      "ShowTotalScpKills": true,
      "ShowTotalFragGrenadeKills": true,
      "ShowTotalEscapedDClass": true,
      "ShowTotalEscapedScientists": true
    }
  },
  "TranslationConfig": {
    "Translation": {
      "Language": "English",
      "Elements": {
        "Player Join": "Player Join",
        "Player Leave": "Player Leave",
        "Player Ban": "Player Ban",
        "ms": "ms",
        "Weapon": "Weapon",
        "Server": "Server",
        "Banned": "Banned",
        "Reason": "Reason",
        "Duration": "Duration",
        "Minutes": "Minute(s)",
        "Hours": "Hour(s)",
        "Days": "Day(s)",
        "Years": "Year(s)",
        "Player Death": "Player Death",
        "Killer": "Killer",
        "Victim": "Victim",
        "Was not Cuffed": "Was not Cuffed",
        "Was Cuffed": "Was Cuffed",
        "Round Summary": "Round Summary",
        "Round Length": "Round Length",
        "Total Kills": "Total Kills",
        "Kills By SCPs": "Kills By SCPs",
        "Kills By Frag Grenades": "Kills By Frag Grenades",
        "Escaped D-Class": "Escaped D-Class",
        "Escaped Scientists": "Escaped Scientists"
      }
    }
  }
}
```

Don't be overwhelmed by the amount, I'll explain everything in a bit.  
The `Prefix` is, as of yet, not important due to the Bot not having any commands as of this point.  
The `BotToken` shall be your own [Discord Bot Token](https://discord.com/developers/).  
The `Port` shall be the Port which your Discord Bot runs on, as well as the Port that your Bot will be listening for requests on.  
The `DiscordActivity` part is for you to be able to customize the bot's presence a bit. `Activity` resembling the status activity, them being:  

Activity  |  ID
------------ | ------------ 
`Playing` | 0 
`Streaming` | 1 
`ListeningTo` | 2 
`Watching` | 3 
`Custom` | 4 
`Competing` | 5 

Small feature, placing `{SLCount}` into the activity name will replace it with the amount of connected SL Servers.  

Regarding the `Guilds`... The way a Guild-entry is structured is as following:  

```json
{
    "GuildID": 0123456789012345,
    "SLFullAddress": "127.0.0.1:8888",
    "DedicatedChannels": {
        "PlayerJoin": 0123456789012345,
        "PlayerLeave": 0123456789012345,
        "RoundSummary": 0123456789012345,
        "PlayerDeath": 0123456789012345,
        "PlayerBan": 0123456789012345
    }
}
```

The valid Event Names can be found [here](EventNames.md). I am always open for suggestions!

Which could look something like this in completion:

```json
    "Guilds": [
        {
            "GuildID": 727996170051518504,
            "ServerPort": 8888,
            "DedicatedChannels": {
                "PlayerJoin": 804830959513239583,
                "PlayerLeave": 804830959513239583,
                "RoundSummary": 805401446471827488,
                "PlayerDeath": 805553243102904350,
                "PlayerBan": 806028507292631040
            }
        },
        {
            "GuildID": 727610880816316536,
            "ServerPort": 8888,
            "DedicatedChannels": {
                "PlayerDeath": 727610980237836289
            }
        },
        {
            "GuildID": 727996170051518504,
            "ServerPort": 7777,
            "DedicatedChannels": {
                "PlayerDeath": 782245173526134814,
                "PlayerJoin": 782687511428333598,
                "PlayerLeave": 782687511428333598
            }
        }
    ]
```

That way you can combine multiple Discord-Servers with different SL-Servers, cross-logging events.  

The next few config parts are the configs for the embeds which are logged.  
Allowing you to toggle whether you want to see the UserId for when a player joins and such.  

Last but not least, you have a small translation config.  
Here you can replace the default-English vocabulary, replacing it with i.e. your own native language.  
Just a small feature, for ease of use.  
Note:  
Leave the left side of words as they are and do not remove entries. Only edit the right side of the translation, so:  
`"Player Join": "This side right here."`

### SyncordPlugin

Each SL server which you want to connect to your SyncordBot has to point towards the IP and Port.  
If you host the bot and the SL server on the same machine, you can simply use the localhost-ip `127.0.0.1`.  
`debugMode` will help you if something is going wrong and you want to know further information.  
If you have an issue with Syncord, you'll likely be asked to activate this debug config and replicate the bug, for us to help you further.  
`autoReconnect` is simply a quality of life feature for you, so the Syncord plugin will automatically reconnect to its Syncord Bot, if connection was lost.  

```yaml
[Syncord]
{
# Debug mode displays important info and errors in the console
debugMode: false
# Address which the Discord-Bot is hosted on
discordBotAddress: 127.0.0.1
# Port which the Discord-Bot is listening to
discordBotPort: 8000
# Whether the Server should try to reconnect when connection is lost
autoReconnect: false
}
```

