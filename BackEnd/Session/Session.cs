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
        /// <summary>
        /// The duration of the session once it has been stopped or paused
        /// </summary>
        public long TotalDuration { get; protected set; }
        /// <summary>
        /// The current duration of the session while the session is still in progress
        /// </summary>
        public long CurrentDuration
        {
            get
            {
                if (InProgress)
                {
                    return TotalDuration + Api.GetEpochTime() - StartTime;
                }
                else
                {
                    return TotalDuration;
                }
            }
        }
        public bool InProgress { get; private set; }

        public Session()
        {
            StartTime = Api.GetEpochTime();
            InProgress = true;
            TotalDuration = 0;
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
                TotalDuration += EndTime - StartTime;
            }
        }
    }
}
