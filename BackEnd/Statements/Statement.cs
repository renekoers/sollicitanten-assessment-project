using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public abstract class Statement
    {
        public static uint MaxStates {get;} = 100;
        internal bool IsInfiniteLoop {get; set;} = false ; 
        internal abstract List<State> ExecuteCommand(Puzzle puzzle);
        internal abstract int GetLines();
    }
}
