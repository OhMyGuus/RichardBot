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
        /// Bot token from https://twitchtokengenerator.com/ 
        /// </summary>
        public string BotToken { get; set; } = "";
       /// <summary>
       /// App token from http://dev.twitch.com
       /// </summary>
        public string AppToken { get; set; } = "";
        /// <summary>
        /// Commands are working only if the botusername is mentioned
        /// </summary>
        public bool MentionOnly { get; set; } = false;
        /// <summary>
        /// List of channels which the bot is connected to, he is always connected to his own channel tho
        /// </summary>
        public List<string> JoinedChannels { get; set; } = new List<string>();
        /// <summary>
        /// Time when the streamer needs to Honor Richard
        /// </summary>
        public double HonorTime { get; set; } = 30;
        /// <summary>
        /// Timeout for saying "avond" in minutes
        /// </summary>
        public int EveningTimeout { get; set; } = 5;
    }
}
