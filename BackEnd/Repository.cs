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
        private static int CreateID()
        {
            return _currentID++;
        }

        private static readonly Dictionary<int, GameSession> GameSessions = new Dictionary<int, GameSession>();

        public static int CreateSession()
        {
            int ID = CreateID();
            GameSessions.Add(ID, new GameSession());
            return ID;
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
