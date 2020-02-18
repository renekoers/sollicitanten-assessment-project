using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Entities.Core;

namespace BackEnd
{
    public class LevelSessionEntity : Entity
    {
        public int LevelNumber { get; protected set; }
		public DateTime Started {get; protected set;}
		public long Duration {get; protected set;}
        public bool InProgress {get; protected set;}
        public LevelSessionEntity(int levelNumber)
        {
			LevelNumber = levelNumber;
			Started = DateTime.UtcNow;
			Duration = 0;
            InProgress = true;
        }
        internal bool End()
        {
            if (InProgress)
			{
				InProgress = false;
				Duration += (long) Math.Ceiling((DateTime.UtcNow - Started).TotalSeconds);
			}
            return !InProgress;
        }
        internal bool Restart()
        {
            if(!InProgress)
            {
                InProgress = true;
                Started = DateTime.UtcNow;
            }
            return InProgress;
        }
    }
}
