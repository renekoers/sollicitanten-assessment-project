using System.Collections.Generic;

namespace BackEnd
{
    public class Repeat : Statement
    {
        private uint _amount;
        private StatementBlock _statements;
        public Repeat(uint amount, Statement[] statements)
            : this(amount, new StatementBlock(statements)) { }

        public Repeat(uint amount, StatementBlock statements)
        {
            _amount = amount;
            _statements = statements;
            this.Command = amount.ToString();
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            for (int i = 0; i < _amount; i++)
            {
                states.AddRange(_statements.ExecuteCommand(puzzle));
                if(_statements.IsInfiniteLoop)
                {
                    this.IsInfiniteLoop = true;
                    return states;
                }
            }
            return states;
        }

        internal override int GetLines()
        {
            return 1 + _statements.GetLines();
        }
        internal override void CompleteProperties()
        {
            if(_amount == 0 || _statements == null || _statements._statements.Length == 0) // If the statementblock is not set correctly.
            {
                if(uint.TryParse(Command, out uint amount))
                {
                    _amount = amount;
                }
                foreach(Statement statement in Code)
                {
                    statement.CompleteProperties();
                }
                _statements = new StatementBlock(Code);
            }
        }
    }
}
