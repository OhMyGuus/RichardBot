using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBotService
{
    public class QuartzServerFactory
    {
        public static QuartzServer CreateServer()
        {
            string typeName = Configuration.ServerImplementationType;

            Type t = Type.GetType(typeName, true);

            QuartzServer retValue = (QuartzServer)Activator.CreateInstance(t);
            return retValue;
        }
    }
}
