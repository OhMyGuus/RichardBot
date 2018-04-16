using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RichardBot.Discord.Config
{
    public class DiscordConfig
    {
        /// <summary>
        /// Auth token of the bot from https://discordapp.com/developers/applications/me
        /// </summary>
        public string BotToken { get; set; } = "";
        /// <summary>
        /// Channels where he is connected to for automated messages you can use @botusername connect to connect
        /// </summary>
        public List<ChannelInfo> connectedChannels { get; set; } = new List<ChannelInfo>();
        /// <summary>
        /// Path to the images 
        /// </summary>
        public string ImagesPath { get; set; } = "images";
        /// <summary>
        /// The json of the weahter api supports only weerlive but you can change the location
        /// </summary>
        public string WeatherApi { get; set; } = "http://weerlive.nl/api/json-10min.php?locatie=Amsterdam";
        /// <summary>
        /// The current game of the Bot
        /// </summary>
        public string Game { get; set; } = "Gebruik @Richard help";
        /// <summary>
        /// Channels where the bot should notify the channels when they go live or stop broadcasting
        /// </summary>
        public List<string> TwitchAlertChannels { get; set; } = new List<string>();
        /// <summary>
        /// The discord channel where debug info needs to be sended to
        /// </summary>
        public ChannelInfo DebugChannel { get; set; } = new ChannelInfo() { GuildId = 433828387354837014, ChannelId = 433828387354837016 };
        /// <summary>
        /// The userid of the admin of the discord bot
        /// </summary>
        public ulong AdminId { get; set; } = 0;
    }
}