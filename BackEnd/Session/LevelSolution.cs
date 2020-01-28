using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class LevelSolution
    {
        public int LevelNumber { get; private set; }
        private readonly Statement[] Code;
        public bool Solved { get; private set; }
        public int Lines
        {
            get
            {
                int lines = 0;
                foreach (Statement statement in Code)
                {
                    lines += statement.GetLines();
                }
                return lines;
            }
        }

        public LevelSolution(int number, Statement[] statements)
        {
            LevelNumber = number;
            Code = statements;
            Puzzle puzzle = new Puzzle(Level.Get(number));
            foreach (Statement statement in Code)
            {
                statement.ExecuteCommand(puzzle);
            }
            Solved = puzzle.Finished;
        }
    }
}
