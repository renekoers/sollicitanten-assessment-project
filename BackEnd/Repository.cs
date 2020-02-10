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
        private static int _currentID = 0;
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

        /// <summary>
        /// This method should get a session of a candidate in a dictionary.!-- -- This needs to be done after HR page is implemented!!!!!!
        /// </summary>
        /// <returns></returns>
        private static int CreateID()
        {
            Console.WriteLine("This method should get a session of a candidate in a dictionary.!-- -- This needs to be done after HR page is implemented!!!!!!");
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
        internal static bool CheckSessionID(int ID)
        {
            Candidates.TryGetValue(ID, out string name);
            return name != null ? UnstartedSessions.Contains(ID) : false;
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

        /// <summary>
        /// Tallies the number of lines of the best solution for the given level number over all sessions
        /// </summary>
        /// <param name="levelNumber"></param>
        /// <returns>A dictionary with as the first int the number of lines and the second int the number of people that solved the level in said amount of lines</returns>
        public static Dictionary<int, int> TallyEveryoneNumberOfLines(int levelNumber)
        {
            Dictionary<int, int> tally = new Dictionary<int, int>();
            foreach (GameSession gameSession in GameSessions.Values)
            {
                LevelSession levelSession = gameSession.GetSession(levelNumber);
                if (levelSession is null)
                {
                    continue;
                }
                LevelSolution leastLinesSolution = levelSession.GetLeastLinesOfCodeSolution();
                if (leastLinesSolution is null)
                {
                    continue;
                }
                int lines = leastLinesSolution.Lines;
                if (tally.ContainsKey(lines))
                {
                    tally[lines]++;
                }
                else
                {
                    tally[lines] = 1;
                }
            }
            return tally;
        }
    }
}
