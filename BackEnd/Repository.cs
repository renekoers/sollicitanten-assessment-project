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
        /// Tallies a given function of the best solution for the given level number over all sessions
        /// </summary>
        /// <param name="levelNumber"></param>
        /// <param name="function"></param>
        /// <returns>A dictionary with as the first int result of function(leastLinesSolution) and the second int the number of people that solved the level with the same info</returns>
        public static Dictionary<int, int> TallyEveryoneBestSolution(int levelNumber, Func<LevelSolution,int> function)
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
                int info = function(leastLinesSolution);
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
        /// Tallies a given function of a level session for the given level number over all sessions
        /// </summary>
        /// <param name="levelNumber"></param>
        /// <param name="function"></param>
        /// <returns>A dictionary with as the first int result of function and the second int the number of people that solved the level with the same info</returns>
        public static Dictionary<int, int> TallyEveryone(int levelNumber, Func<LevelSession, int> function)
        {
            Dictionary<int, int> tally = new Dictionary<int, int>();
            foreach (GameSession gameSession in GameSessions.Values)
            {
                LevelSession levelSession = gameSession.GetSession(levelNumber);
                if (levelSession is null)
                {
                    continue;
                }
                int lines = function(levelSession);
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
