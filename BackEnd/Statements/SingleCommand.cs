using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class SingleCommand : Statement
    {
        private Command _command;
        public SingleCommand(Command command)
        {
            this._command = command;
        }

        internal override List<State> ExecuteCommand(Puzzle puzzle, ICharacter character)
        {
            character.ExecuteCommand(_command);
            return new List<State> { new State(puzzle) };

        }

        internal override int GetLines()
        {
            return 1;
        }
    }
}
