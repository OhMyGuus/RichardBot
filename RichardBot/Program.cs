using RichardBot.Discord;
using RichardBot.Discord.Config;
using RichardBot.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot
{
    class Program
    {
        static void Main(string[] args)
        {
            BotWrapper.Init();
            //TwitchBot twitchBot = new TwitchBot(new TwitchConfig());
            //twitchBot.Connect().Wait();
            // DiscordBot bot = new DiscordBot(UserConfig.Load("config.json"));
            //Task.Run(() => bot.Start());
            Console.ReadLine();
        }
    }
}
