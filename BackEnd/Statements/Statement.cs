using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public abstract class Statement
    {
        internal abstract List<State> ExecuteCommand(Puzzle puzzle);
        internal abstract int GetLines();
    }
}
