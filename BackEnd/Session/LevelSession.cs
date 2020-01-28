using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class LevelSession : Session
    {
        protected List<LevelSolution> Solutions = new List<LevelSolution>();
        public int NumberOfAttempts => Solutions.Count;
        public int LevelNumber { get; protected set; }
        public bool Solved
        {
            get
            {
                foreach (LevelSolution solution in Solutions)
                {
                    if (solution.Solved)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public LevelSession(int levelNumber) : base()
        {
            LevelNumber = levelNumber;
        }

        public void Attempt(LevelSolution solution)
        {
            if (InProgres)
            {
                Solutions.Add(solution);
            }
            else
            {
                throw new InvalidOperationException("Level session has already ended.");
            }
        }
    }
}
