# Syncord

## Description

Syncord is there to offer you a way of logging specific Events from within your SCP SL Server to your Discord Server.  

Special thanks to [GrafDimenzio](https://github.com/GrafDimenzio) for persistently & patiently helping me out with making good use of the [Synapse-API](https://github.com/SynapseSL/Synapse/). 😄

Another thanks to [Exiled's DI](https://github.com/galaxy119/DiscordIntegration/). Why?  
I was a noob at networking in programming before this project. I first looked at DI's implementations, learned a lot.  
However, these influences and inspirations in my code have been overhauled, removing a lot of associations, quirks and more importantly, making it more of my own creation.  

---
## Dependencies

### Syncord Bot
Name  | Version 
------------ | ------------ 
`.NET (Core)` | 5 
`DSharpPlus` | 4.0.0-rc1 
`DSharpPlus.CommandsNext` | 4.0.0-rc1  
`DSharpPlus.Interactivity` | 4.0.0-rc1  
`Serilog` | 2.10.0 
`Serilog.Sinks.Console` | 3.1.1 
`Serilog.Sinks.File` | 4.1.0 
`protobuf-net` | 3.0.73 
`Newtonsoft` | 12.0.3 

### Syncord Info
Name  | Version 
------------ | ------------ 
`.NET Framework` | 4.7.2 
`protobuf-net` | 3.0.73 
`protobuf-net.Core` | 3.0.73

### Syncord Plugin
Name  | Version 
------------ | ------------ 
`.NET Framework` | 4.7.2 
`Synapse` | 2.4.2 
`Newtonsoft.Json` | 12.0.3 

## [Configs]()

This deserves its own dedicated documentation. Please follow the link in the headline.

---

## How to Install

Download the files from the [Latest Release](https://github.com/AlmightyLks/Syncord/releases).  
The Bot only has to be unpacked and stay within its own folder (For your own good, and to make sure of the file integrity).  
It doesn't matter where you put the Bot-Folder, it only has to be on the same system.  
You need to have the `.NET Core 3.1 Runtime` installed.  
For Windows you can easily find it [here](https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.1.404-windows-x64-installer).  
For Linux you can find it [here](https://docs.microsoft.com/en-gb/dotnet/core/install/linux).

For the Server-sided things... You move the `SyncordInfo.dll`, `System.Data.dll`\*, `DSharpPlus.dll`\* & the `Newtonsoft.Json.dll`\*, which are uploaded, into your Synapse\dependencies folder.  
After that, you simply move the `SyncordPlugin.dll` into your Server's plugin folder. Done!

---

## How to use

Using the `syncord.reconnect` permission and either the `Remote Admin Console` or the `Server Console`, you type `syncord connect` in order to connect to the local Syncord Bot.   
After that, you do not have to worry and you can leave the work to the Plugin and the Bot!



\* Note  
These files can be found in the dedicated Plugin-Channel within the [Synapse Discord](https://discord.gg/HWW6s8ggxT).
