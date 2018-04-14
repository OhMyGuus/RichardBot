using Discord;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot.Commands
{
    class CommandMessage
    {
        public string Content { get; set; }
        public String[] SContent { get; set; }
        public int Current { get; set; }
        public ulong Mention { get; set; }
        public string Command { get; set; }
        public int RemainingLength { get { return SContent.Length - Current; } }
        public bool Mentioned { get; set; }
        public CommandMessage(string content, bool twitch = false, string mentionId = "", char seperator = ' ')
        {
            Content = content;
            SContent = Content.Split(seperator);
            if (!twitch)
            {
                ulong mention = 0;
                MentionUtils.TryParseUser(ReadString(), out mention);
                Mention = mention;
                if (mention.ToString() == mentionId)
                {
                    Mentioned = true;
                }
            }
            else
            {
                if (content.StartsWith(mentionId,true,CultureInfo.CurrentCulture))
                {
                    Mentioned = true;
                    ReadString();
                }
                else
                {
                    Mentioned = content.ToLower().Contains(mentionId);
                }
            }

            if (RemainingLength >= 1)
            {
                Command = ReadString();
            }
        }
        public string ReadString(bool readToEnd = false)
        {
            if (RemainingLength >= 1)
            {
                if (!readToEnd)
                {
                    return SContent[Current++];
                }
                else
                {
                    string result = MergeParams(SContent, Current);
                    Current = SContent.Length;
                    return result;
                }
            }
            return "";
        }
        public int ReadInt()
        {
            int result = -1;
            int.TryParse(ReadString(), out result);
            return result;
        }
        private static string MergeParams(string[] Params, int Start)
        {
            StringBuilder MergedParams = new StringBuilder();

            for (int i = 0; i < Params.Length; i++)
            {
                if (i < Start)
                {
                    continue;
                }

                if (i > Start)
                {
                    MergedParams.Append(" ");
                }

                MergedParams.Append(Params[i]);
            }

            return MergedParams.ToString();
        }
    }
}
