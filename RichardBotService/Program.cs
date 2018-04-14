using RichardBot;
using RichardBot.Discord;
using RichardBot.Discord.Config;
using RichardBotService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace RichardBotService
{
    /// <summary>
    /// The server's main entry point.
    /// </summary>
    public static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var container = new UnityContainer();
                //ConfigureContainer(container);

                HostFactory.Run(c =>
                {
                    c.SetServiceName("Richard bot");
                    c.SetDisplayName("Richard bot");
                    c.SetDescription("Discord, twitch bot for lekker spelen fans");

                    c.UseNLog();

                    c.Service(factory =>
                    {
                        BotWrapper.Init().Wait(); 
                        QuartzServer server = QuartzServerFactory.CreateServer();
                        server.Initialize().Wait();
                        return server;
                    });

                    c.EnablePauseAndContinue();

                    //install options
                    c.StartAutomatically(); // Start the service automatically
                    c.RunAsLocalService();  //Runs the service using the local system account.

                });
            }
            catch (Exception oei)
            {
                System.Console.Write(oei.StackTrace);
            }
#if DEBUG
            Console.WriteLine("Service stopped!");
            Console.ReadLine();
#endif
        }


    }
}