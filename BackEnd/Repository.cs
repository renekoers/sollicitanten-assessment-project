using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEnd
{
    /// <summary>
    /// Currently this is a mock for keeping data in memory without any proper persistence, but this should talk to the database, once we have one
    /// </summary>
    public class Repository
    {
        private Repository() { }
        private static int _currentID = 1;
        private static readonly Dictionary<int, GameSession> GameSessions = new Dictionary<int, GameSession>();
        private static readonly Dictionary<int, string> Candidates = new Dictionary<int, string>();
        private static readonly HashSet<int> UnstartedSessions = new HashSet<int>();

        /// <summary>
        /// This method should validate the credentials of HR. This will later be implemented!!!!!!
        /// </summary>
        /// <returns></returns>
        internal static bool ValidateUser(string username, string hashedPassword)
        {
            return true;
        }
        internal static void AddCandidate(string name)
        {
            int ID = CreateID();
            Candidates.Add(ID, name);
            UnstartedSessions.Add(ID);
        }

        private static int CreateID()
        {
            return _currentID++;
        }
        internal static Candidate GetCandidate()
        {
            if(UnstartedSessions.Count == 0){
                return null;
            }
            int ID = UnstartedSessions.First();
            Candidates.TryGetValue(ID, out string name);
            return name != null ? new Candidate(name,ID) : null;
        }
        internal static Candidate GetCandidate(int ID)
        {
            Candidates.TryGetValue(ID, out string name);
            return name != null ? new Candidate(name,ID) : null;
        }
        internal static bool IsUnstarted(int ID)
        {
            Candidates.TryGetValue(ID, out string name);
            return name != null ? UnstartedSessions.Contains(ID) : false;
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
        /// This method creates a list of all IDs of candidates that finished a session after a given time
        /// </summary>
        /// <returns>List of IDs</returns>
        internal static List<int> GetFinishedIDsAfterEpochTime(long epochTime)
        {
            return GameSessions.Where(pair => !pair.Value.InProgress && pair.Value.EndTime>epochTime).Select(pair => pair.Key).ToList();
        }
        /// <summary>
        /// This method finds the first ID of the candidate that ended the session after the given ID.
        /// </summary>
        /// <returns>ID if there exists one</returns>
        internal static int? GetNextIDWhichIsFinished(int ID)
        {
            return Util.Min(GameSessions.Where(pair => !pair.Value.InProgress && pair.Key>ID).Select(pair => pair.Key).ToList());
        }
        /// <summary>
        /// This method finds the last ID of the candidate that ended the session before the given ID.
        /// </summary>
        /// <returns>ID if there exists one</returns>
        internal static int? GetPreviousIDWhichIsFinished(int ID)
        {
            return Util.Max(GameSessions.Where(pair => !pair.Value.InProgress && pair.Key<ID).Select(pair => pair.Key).ToList());
        }
        internal static int? GetLastIDWhichIsFinished()
        {
            return Util.Max(GameSessions.Where(pair => !pair.Value.InProgress).Select(pair => pair.Key).ToList());
        }
        
        [Obsolete("Use StartSession(int ID) instead!")]
        public static int CreateSession()
        {
            int ID = CreateID();
            GameSessions.Add(ID, new GameSession());
            return ID;
        }
        public static bool StartSession(int ID)
        {
            if(UnstartedSessions.Remove(ID)){
                GameSessions.Add(ID, new GameSession());
                return true;
            } else {
                return false;
            }
        }

		public static void CreateTutorialSession()
		{
			if (!GameSessions.ContainsKey(0))
			{
				GameSessions.Add(0, new GameSession());
			}
			else
			{
				GameSessions[0] = new GameSession();
			}

		}

		public static GameSession GetSession(int ID)
		{
			return GameSessions.TryGetValue(ID, out GameSession session) ? session : null;
		}

		public static bool UpdateSession(int ID, GameSession session)
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
