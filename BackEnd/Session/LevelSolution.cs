﻿using System.Collections.Generic;
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
        public List<IState> States;
        [Ignore]
        public int NumberOfStates;
        public LevelSolution(int number, Statement[] statements, long duration=0)
            : this(new Puzzle(Level.Get(number)), statements, duration)
        {
            LevelNumber = number;
        }
        public LevelSolution(Puzzle puzzle, Statement[] statements, long duration=0)
        {
            CodeBlock = new StatementBlock(statements);
            Code = statements;
            Duration = duration;
            Execute(puzzle);
            Solved = puzzle.Finished;
            Lines = CodeBlock.GetLines();
            IsInfiteLoop = CodeBlock.IsInfiniteLoop;
        }
        public void CompleteAllProperties()
        {
            if(CodeBlock==null)
            {
                foreach(Statement statement in Code)
                {
                    statement.CompleteProperties();
                }
                CodeBlock = new StatementBlock(Code);
            }
        }
        public void Execute(Puzzle puzzle)
        {
            States = new List<IState>();
            States.Add(new State(puzzle));
            States.AddRange(CodeBlock.ExecuteCommand(puzzle));
            NumberOfStates = States.Count-1; // -1 because the original state is automaticly added.
        }
    }
}
