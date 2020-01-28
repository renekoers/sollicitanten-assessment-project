using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    /// <summary>
    /// Currently this is a mock for keeping data in memory without any proper persistence, but this should talk to the database, once we have one
    /// </summary>
    public class DatabaseCommunicator
    {
        private DatabaseCommunicator() { }
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
            return GameSessions.TryGetValue(ID, out GameSession value) ? value : null;
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
