using System;

namespace RichardBot.Twitch
{
    internal class BotChannelInfo
    {
        public bool IsLive { get; set; } = false;
        public DateTime? LastHonor { get;  set; }
       
    }
}