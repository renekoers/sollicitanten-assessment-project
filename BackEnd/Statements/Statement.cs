using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Statements
{
    public abstract class Statement
    {
        public abstract List<State> ExecuteCommand(Puzzle puzzle, ICharacter character);
        public abstract int GetLines();
    }
}
