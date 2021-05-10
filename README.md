# Syncord

[![Github All Releases](https://img.shields.io/github/downloads/AlmightyLks/Syncord/total.svg)]()
[![Github All Releases](https://img.shields.io/github/languages/code-size/AlmightyLks/Syncord)]()
[![Github All Releases](https://img.shields.io/tokei/lines/github/AlmightyLks/Syncord)]()
[![Github All Releases](https://img.shields.io/github/license/AlmightyLks/Syncord)]()

## Description

Syncord is there to offer you a way of logging specific Events from within your SCP SL Server to your Discord Server.  

Special thanks to [GrafDimenzio](https://github.com/GrafDimenzio) for persistently & patiently helping me out with making good use of the [Synapse-API](https://github.com/SynapseSL/Synapse/). 😄

Another thanks to [Exiled's DI](https://github.com/galaxy119/DiscordIntegration/). Why?  
I was a noob at networking in programming before this project. I first looked at DI's implementations, getting my hands on using network trafficing myself and learned a lot.  
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
`Synapse` | 2.5.0 
`Newtonsoft.Json` | 12.0.3 

## [Configs](Configs.md)

This deserves its own dedicated documentation. Please follow the link in the headline.

---

## How to Install

Download the files from the [Latest Release](https://github.com/AlmightyLks/Syncord/releases).  
The Bot only has to be unpacked and stay within its own folder (For your own good, and to make sure of the file integrity).  
You do **not** have to install an additional runtime environment, hence the bot's size.  
It doesn't matter where you put the Bot-Folder.  

For the SL server-sided things... 
You move the dependencies\*, which are uploaded, into your Synapse-dependencies folder.  
After that, you simply move the `SyncordPlugin.dll` into your Server's plugin folder. Done!

---

## How to use

The Syncord Plugin has 1 command, with 2 subcommands.  
All of these require the permission `syncord.reconnect`.  

Command Name  | Description 
------------ | ------------ 
`syncord` | Check whether Syncord is currently connected to a Bot.
`syncord connect` | Connect Syncord with the Bot mentioned in the configs.
`syncord disconnect` | Disconnect Syncord from the Bot.

Available in  `Remote Admin Console` and the `Server Console`

\* Note  
These can be found in the dedicated Plugin-Channel within the [Synapse Discord](https://discord.gg/HWW6s8ggxT).
