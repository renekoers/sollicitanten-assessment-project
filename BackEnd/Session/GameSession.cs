using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BackEnd
{
	public class GameSession : Session
	{
		protected Dictionary<int, LevelSession> LevelSessions;
		public int NumberOfSolvedLevels => LevelSessions.Values.ToList().FindAll(s => s.Solved).Count;
		public ISet<int> SolvedLevelNumbers => new HashSet<int>(LevelSessions.Where(pair => pair.Value.Solved).Select(pair => pair.Key).ToList());
		public GameSession() : base()
		{
			LevelSessions = new Dictionary<int, LevelSession>();
		}

		public void AddLevel(LevelSession levelSession)
		{
			if (InProgress)
			{
				LevelSessions.Add(levelSession.LevelNumber, levelSession);
			}
			else
			{
				throw new InvalidOperationException("Game session has already ended.");
			}
		}

		public LevelSession GetSession(int levelNumber)
		{
			return LevelSessions.TryGetValue(levelNumber, out LevelSession session) ? session : null;
		}

		override public void End()
		{
			if (InProgress)
			{
				base.End();
				//Write gamesession data into database.
			}
		}
	}
}
