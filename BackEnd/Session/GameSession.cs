using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class GameSession : Session
    {
        protected Dictionary<int, LevelSession> _levelSessions;

        public GameSession() : base()
        {
            _levelSessions = new Dictionary<int, LevelSession>();
        }

        public void AddLevel(LevelSession levelSession)
        {
            if (InProgres)
            {
                _levelSessions.Add(levelSession.LevelNumber, levelSession);
            }
            else
            {
                throw new InvalidOperationException("Game session has already ended.");
            }
        }

        public LevelSession GetSession(int levelNumber)
        {
            return _levelSessions.TryGetValue(levelNumber, out LevelSession session) ? session : null;
        }
    }
}
