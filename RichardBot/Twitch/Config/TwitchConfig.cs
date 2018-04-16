using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot.Twitch
{
    public class TwitchConfig
    {
        /// <summary>
        /// The bot his username
        /// </summary>
        public string BotUsername { get; set; } = "RichardBot_";
        /// <summary>
        /// The bot admins
        /// </summary>
        public string[] BotAdmins { get; set; } = new string[0];
        /// <summary>
        /// Bot token from https://twitchtokengenerator.com/ 
        /// </summary>
        public string BotToken { get; set; } = "";
       /// <summary>
       /// App token from http://dev.twitch.com
       /// </summary>
        public string AppToken { get; set; } = "";
         /// <summary>
        /// List of channels which the bot is connected to, he is always connected to his own channel tho
        /// </summary>
        public Dictionary<string, ChannelConfig> JoinedChannels { get; set; } = new Dictionary<string, ChannelConfig>();
       
    }
}
