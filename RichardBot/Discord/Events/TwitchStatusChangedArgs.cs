using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot.Discord.Events
{
    public class TwitchStatusChangedArgs : EventArgs
    {
        public string Channel { get; set; }
        public bool Live { get; set; }
        public TimeSpan? Uptime { get; set; }
        public TwitchStatusChangedArgs()
        {

        }
        public TwitchStatusChangedArgs(string channel, bool live = false, TimeSpan? uptime = null)
        {
            this.Channel = channel;
            this.Live = live;
            this.Uptime = uptime;
        }
    }
}
