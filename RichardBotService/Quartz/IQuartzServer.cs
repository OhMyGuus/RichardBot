using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichardBotService
{
    public interface IQuartzServer
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses all activity in scheduler.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes all activity in server.
        /// </summary>
        void Resume();
    }
}
