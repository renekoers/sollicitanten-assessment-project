using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Statements
{
    class SingleCommand : Statement
    {
        private Command _command;
        public SingleCommand(Command command)
        {
            this._command = command;
        }

        public override List<string[][]> ExecuteCommand(Puzzle puzzle, ICharacter character)
        {
            character.ExecuteCommand(_command);
            return new List<string[][]> { puzzle.GetState() };

        }

        public override int GetLines()
        {
            return 1;
        }
    }
}
