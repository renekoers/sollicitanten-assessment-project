using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class GameSession : Session
    {
        protected List<LevelSession> LevelSessions = new List<LevelSession>();

        public GameSession() : base()
        {
        }

        public void AddLevel(LevelSession levelSession)
        {
            if (InProgres)
            {
                LevelSessions.Add(levelSession);
            }
            else
            {
                throw new InvalidOperationException("Game session has already ended.");
            }
        }
    }
}
