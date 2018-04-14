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
    class TwitchHonorJob : IJob
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var bot = BotWrapper.TwitchBot;
                await bot.CheckDabs();

            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }
    }
}
