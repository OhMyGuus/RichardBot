using NLog;
using Quartz;
using RichardBot;
using RichardBot.Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBotService.Job
{
    class SendWeatherJob : IJob
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private DiscordBot bot => BotWrapper.DiscordBot;
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info("Sending dab to channels");
                await bot.SendMessageToAllChannels("<:lekkerRichard:428292200934146049>", bot.GetWeather());
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }
    }
}
