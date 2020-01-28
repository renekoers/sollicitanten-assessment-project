using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class LevelSolution
    {
        public int Number { get; private set; }
        public Statement Code { get; private set; }

        public LevelSolution(int number, Statement statement)
        {
            Number = number;
            Code = statement;
            Puzzle puzzle = new Puzzle(Level.Get(number));
            statement.ExecuteCommand(puzzle);
        }
    }
}
