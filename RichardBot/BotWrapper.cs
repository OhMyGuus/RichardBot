using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RichardBot.Discord;
using RichardBot.Twitch;
using RichardBot.Discord.Config;
using System.IO;
using RichardBot.Config;

namespace RichardBot
{
    public class BotWrapper
    {
        public static DiscordBot DiscordBot;
        public static TwitchBot TwitchBot;
        public static BotsConfig BotsConfig;
        public static async Task Init()
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            BotsConfig = BotsConfig.Load("config.json");
            DiscordBot = new DiscordBot(BotsConfig);
            await DiscordBot.Start();
            TwitchBot = new TwitchBot(BotsConfig);
            await TwitchBot.Connect();

        }
    }
}
