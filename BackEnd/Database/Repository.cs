using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	/// <summary>
	/// Currently this is a mock for keeping data in memory without any proper persistence, but this should talk to the database, once we have one
	/// </summary>
	public class Repository
	{
		private Repository() { }
		private static readonly Dictionary<string, GameSession> GameSessions = new Dictionary<string, GameSession>();

		/// <summary>
		/// This method should validate the credentials of HR. This will later be implemented!!!!!!
		/// </summary>
		/// <returns></returns>
		internal static bool ValidateUser(string username, string hashedPassword)
		{
			return true;
		}

		[Obsolete("Use StartSession(int ID) instead!")]
		public static string CreateSession()
		{
			string ID = "1";
			GameSessions.Add("1", new GameSession());
			return ID;
		}
        /// <summary>
        /// Tallies a given function of a level session for the given level number over all sessions
        /// </summary>
        /// <param name="levelNumber"></param>
        /// <param name="function">Function that maps a level session to an int.</param>
        /// <returns>A dictionary with as the first int result of function and the second int the number of people that solved the level with the same info</returns>
        public static Dictionary<int, int> TallyEveryone(int levelNumber, Func<LevelSession, int> function)
        {
            Dictionary<int, int> tally = new Dictionary<int, int>();
            foreach (GameSession gameSession in GameSessions.Values)
            {
				if(gameSession.InProgress)
				{
					continue;
				}
                LevelSession levelSession = gameSession.GetSession(levelNumber);
                if (levelSession is null || !levelSession.Solved)
                {
                    continue;
                }
                int info = function(levelSession);
                if (tally.ContainsKey(info))
                {
                    tally[info]++;
                }
                else
                {
                    tally[info] = 1;
                }
            }
            return tally;
        }
        /// <summary>
        /// This method creates a dictionary consisting of the number of candidates that did not solve a certain level.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int,int> NumberOfCandidatesNotSolvedPerLevel()
        {
            Dictionary<int,int> amountUnsolved = new Dictionary<int, int>();
            for(int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
                amountUnsolved[levelNumber] = 0;
            }
            foreach(GameSession gameSession in GameSessions.Values)
            {
				if(gameSession.InProgress)
				{
					continue;
				}
                for(int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
                {
                    LevelSession levelSession = gameSession.GetSession(levelNumber);
                    if (levelSession is null || levelSession.Solved)
                    {
                        continue;
                    }
                    amountUnsolved[levelNumber]++;
                }
            }
            return amountUnsolved;
        }

		public static GameSession GetSession(string ID)
		{
			return GameSessions.TryGetValue(ID, out GameSession session) ? session : null;
		}

		public static bool UpdateSession(string ID, GameSession session)
		{
			if (GameSessions.ContainsKey(ID))
			{
				GameSessions[ID] = session;
				return true;
			}
			return false;
		}
	}
}
