﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord;
using Discord.WebSocket;
using RichardBot.Discord.Config;
using System.Data;
using System.IO;
using System.Net;
using RichardBot.Discord.Api;
using Newtonsoft.Json;
using RichardBot.Commands;
using RichardBot.Discord.Events;
using RichardBot.Config;
using DiscordConfig = RichardBot.Discord.Config.DiscordConfig;
namespace RichardBot.Discord
{
    public class DiscordBot : DiscordClientEventHandlers
    {
        public DiscordSocketClient DiscordClient { get; private set; }
        private string commandPrefix;
        private DiscordConfig config => botsConfig.DiscordConfig;
        private BotsConfig botsConfig;
        private Random random = new Random();

        public DiscordBot(BotsConfig config)
        {
            this.botsConfig = config;
            DiscordClient = new DiscordSocketClient();
            DiscordClient.MessageReceived += MessageReceived;
            TwitchStatusChanged += DiscordBot_TwitchStatusChanged; ;
        }

        private async Task DiscordBot_TwitchStatusChanged(TwitchStatusChangedArgs arg)
        {
            if (config.TwitchAlertChannels.Contains(arg.Channel.ToLower()))
            {
                if (arg.Live)
                {
                    if (arg.Uptime?.TotalMinutes < 10)
                        await SendMessageToAllChannels($"Joooo malse makkers, {arg.Channel} is weer LIVE :lekkerSicko: :lekkerSicko: :lekkerRichard:!");
                }
                else
                {
                    await SendMessageToAllChannels("Malse makkers,\nHet streampie is helaas weer voorbij :lekkerAppie:");
                }
            }
        }

        public async Task Start()
        {
            await DiscordClient.StartAsync();
            await DiscordClient.LoginAsync(TokenType.User, config.BotToken);

            while (DiscordClient.ConnectionState == global::Discord.ConnectionState.Connecting || DiscordClient.LoginState == LoginState.LoggingIn)
            {
                await Task.Delay(5);
            }
            commandPrefix = DiscordClient?.CurrentUser?.Mention?.Replace("<@!", "<@");
            await Task.Delay(500);

            await sendDebugMessage("De bot is succesvol opgestart!");
            await DiscordClient.SetGameAsync(config.Game);
        }


        public async Task Restart()
        {
            await DiscordClient.StopAsync();
            await Start();
        }

        public async Task sendDebugMessage(string message)
        {
            var Guild = DiscordClient.GetGuild(config.DebugChannel.GuildId);
            var textChannel = Guild.GetTextChannel(config.DebugChannel.ChannelId);
            await textChannel.SendMessageAsync($"```{message}```");
        }

        public async Task SendMessageToAllChannels(string message = "", Embed embed = null, string file = null)
        {
            foreach (ChannelInfo info in config.connectedChannels)
            {
                try
                {
                    message = parseEmoji(message);
                    var Guild = DiscordClient.GetGuild(info.GuildId);
                    var textChannel = Guild.GetTextChannel(info.ChannelId);
                    if ((!string.IsNullOrEmpty(message) || embed != null) && string.IsNullOrEmpty(file))
                    {
                        var sendedmessage = await textChannel.SendMessageAsync(message);
                        if (embed != null)
                        {
                            await sendedmessage.ModifyAsync(x =>
                            {
                                x.Content = "";
                                x.Embed = embed;
                            });

                        }
                    }
                    if (!string.IsNullOrEmpty(file))
                    {
                        await textChannel.SendFileAsync(file, message);
                    }
                }
                catch (Exception e)
                {
                    await sendDebugMessage($"Error while sending message> ```{e.ToString()}``` ");
                }
            }
        }

        #region commands
        private async Task MessageReceived(SocketMessage message)
        {
            try
            {
                var senderUser = ((global::Discord.WebSocket.SocketGuildUser)message.Author);
                string sender = senderUser.Nickname;
                sender = sender == null ? message.Author.Username : sender;
                CommandMessage command = new CommandMessage(message.Content);
                var klaas = await message.Channel.GetUserAsync(command.Mention);
                if (klaas == null || (klaas.Username.ToLower() != DiscordClient.CurrentUser.Username.ToLower()))
                {
                    return;
                }

                switch (command.Command)
                {
                    case "test":
                        {
                            await message.Channel.SendMessageAsync("test");
                            break;
                        }
                    case "help":
                        {
                            await message.Channel.SendMessageAsync(@"Jooo malse makker, \n" +
                                    "Ik ben er om je te helpen!\n" +
                                    "Gebruik '@richard dab' voor een mooie dab.\n " +
                                    "Gebruik '@richard kokhals' voor een kokhals.\n " +
                                    "Gebruik '@richard meme' voor een leuke meme.\n" +
                                    "Gebruik '@richard weer' voor het weer!");
                            break;
                        }
                    case "connect":
                        {
                            var channelId = message.Channel.Id;
                            var guildId = senderUser.Guild.Id;
                            ChannelInfo channel = new ChannelInfo()
                            {
                                ChannelId = channelId,
                                GuildId = guildId
                            };
                            config.connectedChannels.Add(channel);
                            botsConfig.Save();
                            await message.Channel.SendMessageAsync("Channel toegevoegd.");
                            break;
                        }
                    case "message":
                        {
                            await SendMessageToAllChannels(command.ReadString(true));
                            break;
                        }
                    case "dab":
                        {

                            if (Directory.Exists(config.ImagesPath))
                            {
                                var dabs = Directory.GetFiles(config.ImagesPath).Where(o => o.Contains("dab")).ToArray();
                                if (dabs.Length > 0)
                                {
                                    var dab = dabs.ElementAt(random.Next(dabs.Length));
                                    Task.Run(async () => await message.Channel.SendFileAsync(dab));
                                }
                                else
                                {
                                    await sendDebugMessage(config.ImagesPath + " no memes found found??");
                                }
                            }
                            else
                            {
                                await sendDebugMessage(config.ImagesPath + " not found??");
                            }
                            break;
                        }
                    case "kokhals":
                        {
                            if (Directory.Exists(config.ImagesPath))
                            {
                                var dabs = Directory.GetFiles(config.ImagesPath).Where(o => o.Contains("kokhals")).ToArray();
                                if (dabs.Length > 0)
                                {
                                    var dab = dabs.ElementAt(random.Next(dabs.Length));
                                    Task.Run(async () => await message.Channel.SendFileAsync(dab));
                                }
                            }
                            break;
                        }
                    case "meme":
                        {
                            if (Directory.Exists(config.ImagesPath))
                            {
                                var memes = Directory.GetFiles(config.ImagesPath).Where(o => o.Contains("meme")).ToArray();
                                if (memes.Length > 0)
                                {
                                    var meme = memes.ElementAt(random.Next(memes.Length));
                                    Task.Run(async () => await message.Channel.SendFileAsync(meme));
                                }
                            }
                            break;
                        }
                    case "weer":
                        {
                            var weatherMessage = await message.Channel.SendMessageAsync(":robot:");
                            await weatherMessage.ModifyAsync(x =>
                            {
                                x.Content = "";
                                x.Embed = GetWeather();
                            });
                            break;
                        }
                    case "broadcastweer":
                        {
                            var weer = GetWeather();
                            await SendMessageToAllChannels(":robot:", weer);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                await sendDebugMessage(e.ToString());
            }
        }
        public string GetFile(string prefix)
        {
            var dabs = Directory.GetFiles(config.ImagesPath).Where(o => o.Contains(prefix + "_")).ToArray();
            if (dabs.Length > 0)
            {
                return dabs.ElementAt(random.Next(dabs.Length));
            }
            return "";
        }
        public Embed GetWeather()
        {
            // http://weerlive.nl/api/json-10min.php?locatie=52.0910879,5.1124231
            EmbedBuilder embedbuilder = new EmbedBuilder();
            Liveweer weather = new Liveweer();
            try
            {
                using (WebClient client = new WebClient())
                {
                    var jsonWeather = client.DownloadString(config.WeatherApi);
                    weather = JsonConvert.DeserializeObject<Weather>(jsonWeather).GetLiveWeer();
                }
                embedbuilder.Title = $"Jooo malse makkers! \r\n";
                embedbuilder.WithThumbnailUrl("https://cdn.discordapp.com/attachments/384434868055965696/433847402764697610/Untitled.png");
                embedbuilder.WithDescription("Ik ben er weer en het is weer tijd voor het weer!\n .");
                embedbuilder.AddField("Verwachting:", weather.verw);
                embedbuilder.AddField("temperatuur", $"De temperatuur is tussen de {weather.d0tmin} en {weather.d0tmax} graden");
                embedbuilder.AddField("Zonkans:", $"De kans op zon is {weather.d0zon}%");
                embedbuilder.AddField("Neerslagkans :", $"De kans op neerslag is {weather.d0neerslag}%");
                embedbuilder.AddField("Windracht :", $"Windrkracht is {weather.d0windkmh} km/h");
                embedbuilder.AddField("Windrichting :", $"De wind komt vanuit {weather.d0windr}");
                embedbuilder.AddField("Luchtdruk :", $"De luchtdruk is {weather.luchtd} milibar");
                embedbuilder.AddField("Het is vandaag:", $" {weather.d0weer}");
                embedbuilder.WithFooter("Tot de volgende keer!");

                embedbuilder.WithColor(Color.Red);
                return embedbuilder.Build();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private string parseEmoji(string input)
        {
            input = input.Replace(":lekkerRichard:", "<:lekkerRichard:428292200934146049>");
            input = input.Replace(":lekkerAppie:", "<:lekkerappie:323177514648469514>");
            input = input.Replace(":lekkerSicko:", "<:lekkerSicko:397984457145188362>");
            return input;
        }
        #endregion



    }
}
