using NLog;
using RichardBot.Commands;
using RichardBot.Config;
using RichardBot.Discord.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Services;

namespace RichardBot.Twitch
{
    public class TwitchBot
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        readonly ConnectionCredentials credentials;
        private TwitchClient client = new TwitchClient();
        private TwitchConfig config => botConfigs.TwitchConfig;
        private BotsConfig botConfigs;
        private TwitchAPI api = new TwitchAPI();
        private Dictionary<string, BotChannelInfo> botChannelInfos = new Dictionary<string, BotChannelInfo>();
        private Dictionary<string, DateTime> botEveningTimeout = new Dictionary<string, DateTime>();

        public TwitchBot(BotsConfig botconfig)
        {
            this.botConfigs = botconfig;
            credentials = new ConnectionCredentials(config.BotUsername, config.BotToken);
            client.Initialize(credentials, config.BotUsername);
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnConnected += Client_OnConnected;
            client.OnConnectionError += Client_OnConnectionError;
            client.OnLog += Client_OnLog;
            //client.ChatThrottler = new MessageThrottler(client, 20, TimeSpan.FromSeconds(30));
            //client.WhisperThrottler = new MessageThrottler(client, 20, TimeSpan.FromSeconds(30));
            client.OnLeftChannel += Client_OnLeftChannel;
            client.OnJoinedChannel += Client_OnJoinedChannel;
        }


        public async Task Connect()
        {
            await api.InitializeAsync(config.AppToken, config.BotToken);
            client.Connect();
        }
        int k = 0;
        private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Username == client.TwitchUsername)
            {
                return;
            }
            CommandMessage command = new CommandMessage(e.ChatMessage.Message, true, $"@{config.BotUsername.ToLower()}");
            ChatMessage message = e.ChatMessage;
            string channelNameL = message.Channel.ToLower();
            ChannelConfig channelConfig = config.JoinedChannels.ContainsKey(message.Channel.ToLower()) ? config.JoinedChannels[channelNameL] : new ChannelConfig();
            if (channelConfig.MentionOnly && !command.Mentioned)
            {
                return;
            }

            switch (command.Command.ToLower())
            {
                case "!uptime":
                    {
                        var uptime = GetUptime(message.Channel);
                        client.SendMessage(message.Channel, uptime == null ? $"{message.Channel} is op dit moment niet aan het streamen! lekkerKonkie" : $"{message.Channel} is al {(int)(uptime?.TotalMinutes ?? 0)} minuten aan het streamen");
                        break;
                    }
                case "!join":
                    {
                        string channelName = command.ReadString();
                        if (!string.IsNullOrEmpty(channelName) && !config.JoinedChannels.ContainsKey(channelName.ToLower()))
                        {
                            var channel = api.Users.v5.GetUserByNameAsync(channelName).Result.Matches;
                            if (channel != null && channel.Length >= 1)
                            {
                                config.JoinedChannels.Add(channelName.ToLower(), new ChannelConfig());
                                client.JoinChannel(channelName);
                                client.SendMessage(message.Channel, $"lekkerRichard ik kom naar het kanaal {channelName} lekkerSicko ");
                                botConfigs.Save();
                            }
                        }
                        break;
                    }
                case "lekkerrichard":
                    {
                        if (channelConfig.RichardEmoteEnabled)
                        {
                            client.SendMessage(message.Channel, $"lekkerRichard lekkerRichard lekkerRichard lekkerSicko {k++}");
                        }
                        break;
                    }
                case "avond":
                    {
                        if (channelConfig.EveningEnabled && (!botEveningTimeout.ContainsKey(message.UserId) || (DateTime.Now - botEveningTimeout[message.UserId]).TotalMinutes > channelConfig.EveningTimeout))
                        {
                            botEveningTimeout[message.UserId] = DateTime.Now;
                            client.SendMessage(message.Channel, $"/me Avond @{e.ChatMessage.DisplayName} malse makker! lekkerRichard lekkerDag");
                        }
                        break;
                    }
                case "middag":
                    {
                        if (channelConfig.EveningEnabled && !botEveningTimeout.ContainsKey(message.UserId) || (DateTime.Now - botEveningTimeout[message.UserId]).TotalMinutes > channelConfig.EveningTimeout)
                        {
                            botEveningTimeout[message.UserId] = DateTime.Now;
                            client.SendMessage(message.Channel, $"/me Middag @{e.ChatMessage.DisplayName} malse makker! lekkerRichard lekkerDag");
                        }
                        break;
                    }
                case "!enable":
                case "!disable":
                    {
                        if (IsAdmin(message))
                        {
                            switch (command.ReadString().ToLower())
                            {
                                case "emote":
                                    {
                                        channelConfig.RichardEmoteEnabled = !channelConfig.RichardEmoteEnabled;
                                        SendMessage($"Ik reageer " + (channelConfig.RichardEmoteEnabled? "weer" : "niet meer") + " op lekkerRichard lekkerAppie", message.Channel);
                                        break;
                                    }
                                case "evening":
                                case "avond":
                                    {
                                        channelConfig.EveningEnabled = !channelConfig.EveningEnabled;
                                        SendMessage($"Ik reageer " + (channelConfig.EveningEnabled ? "Weer" : "niet meer") + " op avond lekkerAppie",message.Channel);
                                        break;
                                    }
                            }
                            botConfigs.Save();
                        }
                        break;
                    }
                
                default:
                    {
                        //Need to move stuff around to make this less italian rrly need to 

                        if (message.Message.ToLower().Contains(" avond") || message.Message.ToLower().Contains(" avond "))
                        {
                            goto case "avond";
                        }
                        else if (message.Message.ToLower().Contains(" middag") || message.Message.ToLower().Contains(" middag "))
                        {
                            goto case "middag";
                        }
                        break;
                    }
            }
        }
        private bool IsAdmin(ChatMessage message)
        {
            return message.IsModerator || message.Username.ToLower() == config.BotUsername.ToLower() || config.BotAdmins.Contains(message.Username.ToLower());

        }
        public async Task CheckDabs()
        {
            foreach (var channel in client.JoinedChannels)
            {
                var channelConfig = config.JoinedChannels.ContainsKey(channel.Channel.ToLower()) ? config.JoinedChannels[channel.Channel.ToLower()] : new ChannelConfig();
                var info = GetBotChannelInfo(channel.Channel);
                var uptime = GetUptime(channel.Channel);
                if (!uptime.HasValue)
                {
                   if (info.IsLive)
                    {
                        BotWrapper.DiscordBot.OnTwitchStatusChanged(new TwitchStatusChangedArgs(channel.Channel, false));
                    }
                    info.LastHonor = null;
                    info.IsLive = false;
                }
                else
                {
                    if (!info.IsLive)
                    {
                        BotWrapper.DiscordBot.OnTwitchStatusChanged(new TwitchStatusChangedArgs(channel.Channel, true, uptime));
                        info.IsLive = true;
                    }
                    TimeSpan noHonorTime = info.LastHonor.HasValue ? DateTime.Now - info.LastHonor.Value : uptime.Value;
                    if ((noHonorTime.TotalMinutes > channelConfig.HonorTime))
                    {
                        client.SendMessage(channel, $"/me Hey malse makkers,\n Jullie zijn al weer {(int)noHonorTime.TotalMinutes} minuten live zonder Richard te eren geef hem even een vingertje lekkerRichard lekkerKonkie");
                        info.LastHonor = DateTime.Now;
                    }
                }
            }
            //SendMessage("Vergeten jullie niet Richard te eren door een dab te doen lekkerRichard lekkerKonkie");
        }
        private BotChannelInfo GetBotChannelInfo(string channel)
        {
            if (botChannelInfos.ContainsKey(channel.ToLower()))
            {
                return botChannelInfos[channel.ToLower()];
            }
            else
            {
                botChannelInfos.Add(channel.ToLower(), new BotChannelInfo());
                return botChannelInfos[channel.ToLower()];
            }
        }

        public TimeSpan? GetUptime(string channel)
        {
            var user = api.Users.v5.GetUserByNameAsync(channel).Result.Matches.FirstOrDefault();
            var uptime = api.Streams.v5.GetUptimeAsync(user.Id).Result;
            bool isStreaming = api.Streams.v5.BroadcasterOnlineAsync(user.Id).Result;
            return uptime;
        }
        public void SendMessage(string message, string channelId = null)
        {
            if (!string.IsNullOrEmpty(channelId))
            {
                client.SendMessage(channelId, message);
            }
            else
            {
                foreach (var channel in client.JoinedChannels)
                {
                    client.SendMessage(channel, message);
                }
            }
        }

        #region log
        private void Client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            logger.Trace(e.Data);
        }

        private void Client_OnConnectionError(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
        {
            logger.Trace("Connection error!");
        }

        private async void Client_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            await BotWrapper.DiscordBot.sendDebugMessage($"Joined chanenel {e.Channel}");
        }

        private async void Client_OnLeftChannel(object sender, TwitchLib.Client.Events.OnLeftChannelArgs e)
        {
            await BotWrapper.DiscordBot.sendDebugMessage($"Left chanenel {e.Channel}");
        }

        private void Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            //    client.ChatThrottler = new MessageThrottler(client, 20, TimeSpan.FromSeconds(30));
            logger.Trace("Connected to twitch");
            //client.WhisperThrottler.StartQueue();
            //client.ChatThrottler.StartQueue();
            foreach (var channel in config.JoinedChannels.Keys)
            {
                logger.Trace($"Joining {channel}");
                client.JoinChannel(channel);
            }
        }
        #endregion
    }
}
