using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot.Discord.Events
{
    public class DiscordClientEventHandlers
    {
        public event Func<TwitchStatusChangedArgs, Task> TwitchStatusChanged;
        public virtual void OnTwitchStatusChanged(TwitchStatusChangedArgs e)
        {
            TwitchStatusChanged?.Invoke(e);
        }
    }
}
