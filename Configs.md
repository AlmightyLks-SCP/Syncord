## Configs

### **Each individual config will re-create itself with its default values when not existent**  

---
### Overview:

- [Syncord Bot](Configs.md#)
  - [Bot-Config](Configs.md#Bot-Config)
  - [Guild-Config](Configs.md#Guild-Config)
  - [Alias-Config](Configs.md#Alias-Config)
  - [Translation-Config](Configs.md#Translation-Config)
- [Syncord Plugin](Configs.md#SyncordPlugin)

---

### [Bot-Config](#Bot-Config)

The default configs for this look as following:  

```json
{
  "Prefix": "!",
  "Bot Token": "Your Bot Token here",
  "Port": 8000,
  "Remote Connection": false,
  "Discord Activity": {
    "Name": "{SLCount} Server/s",
    "Activity": 3
  },
  "Embed Configs": {
    "Display Server IP / Alias": false,
    "Player Joined / Left": {
      "Show User ID": true,
      "Show Ping": true,
      "Show IP (or Do Not Track)": false
    },
    "Player Death": {
      "Show User ID": true
    },
    "Round Summary": {
      "Show Round Length": true,
      "Show Total Kills": true,
      "Show Total Scp Kills": true,
      "Show Total Frag Grenade Kills": true,
      "Show Total Escaped DClass": true,
      "Show Total Escaped Scientists": true
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


The `Embed Configs` part is pretty self-explanatory and it lets allows you to customise some the logging messages to a certain extend.  

---

### [Guild-Config](#Guild-Config)

The default configs for this look as following:  

```json
{
  "Guilds": []
}
```

Note: `[]` indicates a collection of something.  
Here you can insert your entries where you link the discord guilds, the sl servers and the desired logs together.  
The way a Guild-entry is structured is as following:  

```json
{
    "GuildID": 0123456789012345,
    "Full SL Address": "127.0.0.1:8888",
    "Dedicated Channels": {
        "PlayerJoin": 0123456789012345,
        "PlayerLeave": 0123456789012345,
        "RoundSummary": 0123456789012345,
        "PlayerDeath": 0123456789012345,
        "PlayerBan": 0123456789012345
    }
}
```

The valid event names can be found [here](EventNames.md). I am always open for suggestions!  
So that would be for example:  

```json
{
  "Guilds": [
    {
      "GuildID": 121212121212,
      "Full SL Address": "127.0.0.1:8888",
      "Dedicated Channels": {
          "PlayerJoin": 0123456789012345,
          "PlayerLeave": 0123456789012345,
          "RoundSummary": 0123456789012345,
          "PlayerDeath": 0123456789012345,
          "PlayerBan": 0123456789012345
        }
    },
    {
      "GuildID": 56565656565656,
      "Full SL Address": "127.0.0.1:6726",
      "Dedicated Channels": {
          "PlayerJoin": 0123456789012345,
          "PlayerLeave": 0123456789012345,
        }
    }
  ]
}
```

That way you can combine multiple Discord-Servers with different SL-Servers, cross-logging events.  

---

### [Alias-Config](#Alias-Config)

The default configs for this look as following:  

```json
{
  "Aliases": {}
}
```

Here you can add Aliases for your SL servers, for a friendlier representation for your server ip.  
It is a simple dictionary, so adding some examples it could look something like this:  

```json
{
  "Aliases": {
    "127.0.0.1:7777" : "Wholesome's place 1",
    "127.0.0.1:8888" : "Wholesome's place 2"
  }
}
```

---

### [Translation-Config](#Translation-Config)

Last but not least, you have a small translation config.  
Here you can replace the default-English vocabulary, replacing it with i.e. your own native language.  
Just a small feature, for quality of life.  
Note:  
Leave the left side of words as they are and do not remove entries. Only edit the right side of the translation, so as follows:  
`"Player Join": "This side right here."`

---

### [SyncordPlugin](#SyncordPlugin)

Each SL server which you want to connect to your SyncordBot has to point towards the IP and Port.  
If you host the bot and the SL server on the same machine, you can simply use the localhost-ip `127.0.0.1`.  
As soon as you use localhost, it will recognize that and will furthermore identify the sl server via the localhost ip.
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
# Whether the Server should try to connect when there is no connection
autoConnect: false
}
```

