namespace RichardBot.Twitch
{
    public class ChannelConfig
    {
        /// <summary>
        /// Commands are working only if the botusername is mentioned
        /// </summary>
        public bool MentionOnly { get; set; } = false;
        /// <summary>
        /// Time when the streamer needs to Honor Richard
        /// </summary>
        public double HonorTime { get; set; } = 30;
        /// <summary>
        /// Timeout for saying "avond" in minutes
        /// </summary>
        public int EveningTimeout { get; set; } = 5;
        /// <summary>
        /// Evening enabled
        /// </summary>
        public bool EveningEnabled { get; set; } = true;
        /// <summary>
        ///  Richard emote enabled
        /// </summary>
        public bool RichardEmoteEnabled { get; set; } = true;

    }
}