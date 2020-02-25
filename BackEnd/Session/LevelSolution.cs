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
        [Ignore]
        public StatementBlock CodeBlock;
        public long Duration {get; private set;}
        public bool Solved { get; private set; }
        public int Lines;
        public bool IsInfiteLoop;
        [Ignore]
        public List<IState> States { get; private set; }
        [Ignore]
        public int NumberOfStates;

        public LevelSolution(int number, Statement[] statements)
            : this(number, statements, 0){}
        public LevelSolution(int number, Statement[] statements, long duration)
        {
            LevelNumber = number;
            CodeBlock = new StatementBlock(statements);
            Code = statements;
            Duration = duration;
            Puzzle puzzle = new Puzzle(Level.Get(number));
            States = new List<IState>();
            States.Add(new State(puzzle));
            States.AddRange(CodeBlock.ExecuteCommand(puzzle));
            NumberOfStates = States.Count-1; // -1 because the original state is automaticly added.
            Solved = puzzle.Finished;
            Lines = CodeBlock.GetLines();
            IsInfiteLoop = CodeBlock.IsInfiniteLoop;
        }
        public void ConvertCodeToOriginalTypes()
        {
            if(CodeBlock==null)
            {
                List<Statement> codeAsList = new List<Statement>();
                foreach(Statement statement in Code)
                {
                    codeAsList.Add(statement.GetStatementAsOriginalType());
                }
                CodeBlock = new StatementBlock(codeAsList.ToArray());
            }
        }
    }
}
