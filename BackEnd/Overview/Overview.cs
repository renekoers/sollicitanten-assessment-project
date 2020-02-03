using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Overview{
        public List<LevelOverview> levels = new List<LevelOverview>();
        public Overview(GameSession session){
            for(int levelNumber=1; levelNumber<Level.TotalLevels; levelNumber++){
                LevelSession level = session.GetSession(levelNumber);
                if(level==null){
                    levels.Add(new LevelOverview(levelNumber));
                } else {
                    levels.Add(new LevelOverview(level));
                }
            }
        }

    }
}