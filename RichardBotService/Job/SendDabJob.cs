using NLog;
using Quartz;
using RichardBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBotService.Job
{
    class SendDabJob : IJob
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var bot = BotWrapper.DiscordBot;
                logger.Info("Sending dab to channels");
                await BotWrapper.DiscordBot.SendMessageToChannels("Joo malse makkers! \n Vergeet niet even te dabben!", null, bot.GetFile("dab"));
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }
    }
}
