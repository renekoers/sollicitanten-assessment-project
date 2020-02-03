using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BackEnd
{
    public class LevelSession : Session
    {
        protected List<LevelSolution> Solutions = new List<LevelSolution>();
        public int NumberOfAttempts => Solutions.Count;
        public int NumberOfAttemptsForFirstSolved => Solutions.FindIndex(s => s.Solved) + 1;
        public int LevelNumber { get; protected set; }
        public bool Solved => Solutions.Any(s => s.Solved);
        public LevelSolution FindFirstSolution()
        {
            return Solutions.Find(s => s.Solved);
        }
        public LevelSolution GetLeastLinesOfCodeSolution()
        {
            return Util.Min(Solutions.FindAll(s => s.Solved), s => s.Lines);
        }
        public LevelSolution GetLeastNumberOfStatesSolution()
        {
            return Util.Min(Solutions.FindAll(s => s.Solved), s => s.NumberOfStates);
        }

        public LevelSession(int levelNumber) : base()
        {
            LevelNumber = levelNumber;
        }

        public void Attempt(LevelSolution solution)
        {
            if (InProgress)
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
