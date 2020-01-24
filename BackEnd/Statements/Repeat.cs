using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    class Repeat : Statement
    {
        private uint _amount;
        private Statement[] _statements;
        public Repeat(uint amount, Statement[] statements)
        {
            _amount = amount;
            _statements = statements;
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle, ICharacter character)
        {
            List<State> states = new List<State>();
            for (int i = 0; i < _amount; i++)
            {
                foreach  (Statement statement in _statements)
                {
                    states.AddRange(statement.ExecuteCommand(puzzle, character);
                }
            }
            return states;
        }

        internal override int GetLines()
        {
            int lines = 1;
            foreach (Statement statement in _statements)
            {
                lines += statement.GetLines();
            }
            return lines;
        }
    }
}
