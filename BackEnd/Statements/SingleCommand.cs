using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    class SingleCommand : Statement
    {
        private Command _command;
        public SingleCommand(Command command)
        {
            this._command = command;
        }

        public override List<State> ExecuteCommand(Puzzle puzzle, ICharacter character)
        {
            character.ExecuteCommand(_command);
            return new List<State> { new State(puzzle) };

        }

        public override int GetLines()
        {
            return 1;
        }
    }
}
