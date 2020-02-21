using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
    public abstract class Statement : Entity
    {
        [Ignore]
        public static uint MaxStates {get;} = 100;
        [Ignore]
        internal bool IsInfiniteLoop {get; set;} = false ; 
        public string StatementType => this.GetType().ToString();
        public ConditionEntity Condition;
        public Statement[] Code;
        public string Command;
        internal abstract List<State> ExecuteCommand(Puzzle puzzle);
        internal abstract int GetLines();
    }
}
