using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public abstract class Session
    {
        /// <summary>
        /// Times are in milliseconds Epoch
        /// </summary>
        public long StartTime { get; protected set; }
        public long EndTime { get; protected set; }
        public long Duration { get; protected set; }
        public bool InProgress { get; private set; }

        public Session()
        {
            StartTime = Api.GetEpochTime();
            InProgress = true;
            Duration = 0;
        }

        public void Pause()
        {
            End();
        }

        public void Restart()
        {
            if (!InProgress)
            {
                StartTime = Api.GetEpochTime();
                InProgress = true;
            }
        }

        public void End()
        {
            if (InProgress)
            {
                InProgress = false;
                EndTime = Api.GetEpochTime();
                Duration += EndTime - StartTime;
            }
        }
    }
}
