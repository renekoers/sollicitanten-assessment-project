using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class LevelSolution
    {
        public int LevelNumber { get; private set; }
        private readonly StatementBlock Code;
        public bool Solved { get; private set; }
        public int Lines => Code.GetLines();
        public List<IState> States { get; private set; }

        public LevelSolution(int number, Statement[] statements)
            : this(number, new StatementBlock(statements)) { }
        public LevelSolution(int number, StatementBlock statements)
        {
            LevelNumber = number;
            Code = statements;
            Puzzle puzzle = new Puzzle(Level.Get(number));
            States = new List<IState>();
            States.AddRange(Code.ExecuteCommand(puzzle));
            Solved = puzzle.Finished;
        }
    }
}
