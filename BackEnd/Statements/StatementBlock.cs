using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class StatementBlock : Statement
    {
        private readonly Statement[] _statements;

        public StatementBlock(Statement[] statements)
        {
            _statements = statements;
        }

        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            foreach (Statement statement in _statements)
            {
                states.AddRange(statement.ExecuteCommand(puzzle));
            }
            return states;
        }

        internal override int GetLines()
        {
            int lines = 0;
            foreach (Statement statement in _statements)
            {
                lines += statement.GetLines();
            }
            return lines;
        }
    }
}
