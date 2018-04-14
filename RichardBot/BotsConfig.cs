using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RichardBot.Discord.Config;
using RichardBot.Twitch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot.Config
{
    public class BotsConfig
    {
        private string file = "config.json";
        public TwitchConfig TwitchConfig { get; set; } = new TwitchConfig();
        public DiscordConfig DiscordConfig { get; set; } = new DiscordConfig();

        public static BotsConfig Load(string file)
        {
            BotsConfig config = new BotsConfig() { file = file };
            if (!File.Exists(file))
            {
                config.Save();
            }
            else
            {
                string configJson = File.ReadAllText(file);
                config = JsonConvert.DeserializeObject<BotsConfig>(configJson);
                config.file = file;
            }
            return config;
        }
        public void Save()
        {
            string configJson = JsonConvert.SerializeObject(this);
            File.WriteAllText(file, JToken.Parse(configJson).ToString(Formatting.Indented));
        }
    }
}
