using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Repeat : Statement
    {
        private readonly uint _amount;
        private readonly Statement[] _statements;
        public Repeat(uint amount, Statement[] statements)
        {
            _amount = amount;
            _statements = statements;
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            for (int i = 0; i < _amount; i++)
            {
                foreach  (Statement statement in _statements)
                {
                    states.AddRange(statement.ExecuteCommand(puzzle));
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
