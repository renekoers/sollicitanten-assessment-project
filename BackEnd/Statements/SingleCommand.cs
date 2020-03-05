using System.Collections.Generic;

namespace BackEnd
{
    public class SingleCommand : Statement
    {
        private Command _command;
        public SingleCommand(Command command)
        {
            this._command = command;
            this.Command = command.ToString();
        }

        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            puzzle.Character.ExecuteCommand(_command);
            return new List<State> { new State(puzzle) };
        }

        internal override int GetLines()
        {
            return 1;
        }
        internal override void CompleteProperties()
        {
            if(_command == 0 && !Command.Equals((Command)0)) // If _command is not correctly set
            {
                if(BackEnd.Command.TryParse(Command, out Command command))
                {
                    _command = command;
                }
            }
        }
    }
}
