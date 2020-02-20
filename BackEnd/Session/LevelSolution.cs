using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
    /// <summary>
    /// Represents an attempt at solving a level
    /// </summary>
    public class LevelSolution : Entity
    {
        [Ignore]
        public int LevelNumber { get; private set; }
        public Statement[] Code;
        public long Duration {get; private set;}
        public bool Solved { get; private set; }
        public int Lines => new StatementBlock(Code).GetLines();
        public bool IsInfiteLoop;
        [Ignore]
        public List<IState> States { get; private set; }
        public int NumberOfStates => States.Count;

        public LevelSolution(int number, Statement[] statements)
            : this(number, statements, 0){}
        public LevelSolution(int number, Statement[] statements, long duration)
        {
            LevelNumber = number;
            Code = statements;
            Duration = duration;
            Puzzle puzzle = new Puzzle(Level.Get(number));
            States = new List<IState>();
            States.Add(new State(puzzle));
            StatementBlock blockCode = new StatementBlock(statements);
            States.AddRange(blockCode.ExecuteCommand(puzzle));
            Solved = puzzle.Finished;
            IsInfiteLoop = blockCode.IsInfiniteLoop;
        }
    }
}
