using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Statements
{
    abstract class Statement
    {
        public abstract List<string[][]> ExecuteCommand(Puzzle puzzle, ICharacter character);
        public abstract int GetLines();
    }
}
