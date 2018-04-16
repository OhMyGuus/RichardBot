using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBot.Discord
{
    public enum MemeType
    {
        Meme,
        Dab,
        Vinger,
        kokhals
    }
    static class MemeTypeHelper
    {
        public static MemeType FromString(string input)
        {
            switch (input.ToLower())
            {
                case "meme":
                    return MemeType.Meme;
                case "dab":
                    return MemeType.Dab;
                case "vinger"://dutch localisation
                case "vingertje":
                case "finger":
                    return MemeType.Vinger;
                case "kokhals":
                    return MemeType.kokhals;
                default:
                    return MemeType.Meme;
            }
        }
    }
}
