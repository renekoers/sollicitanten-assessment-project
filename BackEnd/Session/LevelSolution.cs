using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    /// <summary>
    /// Represents an attempt at solving a level
    /// </summary>
    public class LevelSolution
    {
        public int LevelNumber { get; private set; }
        private readonly StatementBlock Code;
        public long Duration {get; private set;}
        public bool Solved { get; private set; }
        public int Lines => Code.GetLines();
        public List<IState> States { get; private set; }
        public int NumberOfStates => States.Count;

        public LevelSolution(int number, Statement[] statements)
            : this(number, new StatementBlock(statements)) { }
        public LevelSolution(int number, StatementBlock statements)
            : this(number, statements, 0){}
        public LevelSolution(int number, StatementBlock statements, long duration)
        {
            LevelNumber = number;
            Code = statements;
            Duration = duration;
            Puzzle puzzle = new Puzzle(Level.Get(number));
            States = new List<IState>();
            States.Add(new State(puzzle));
            States.AddRange(Code.ExecuteCommand(puzzle));
            Solved = puzzle.Finished;
        }
    }
}
