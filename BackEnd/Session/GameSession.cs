using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Entities.Core;

namespace BackEnd
{
	public class GameSession : Entity
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
		protected Dictionary<int, LevelSession> LevelSessions;
		public int NumberOfSolvedLevels => LevelSessions.Values.ToList().FindAll(s => s.Solved).Count;
		public ISet<int> SolvedLevelNumbers => new HashSet<int>(LevelSessions.Where(pair => pair.Value.Solved).Select(pair => pair.Key).ToList());
		public GameSession()
		{
			LevelSessions = new Dictionary<int, LevelSession>();
			StartTime = Api.GetEpochTime();
			InProgress = true;
			TotalDuration = 0;
		}
		public LevelSession GetSession(int levelNumber)
		{
			return LevelSessions.TryGetValue(levelNumber, out LevelSession session) ? session : null;
		}

		virtual public void End()
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
