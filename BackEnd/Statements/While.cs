using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class While : ConditionalStatement
    {
        private readonly ConditionParameter _parameter;
        private readonly ConditionValue _value;
        private readonly bool _isTrue;
        private readonly StatementBlock _statements;
        public While(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statements)
            : this(parameter, value, isTrue, new StatementBlock(statements)) { }
        public While(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statements)
        {
            this._parameter = parameter;
            this._value = value;
            this._isTrue = isTrue;
            this._statements = statements;
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            while (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue)
            {
                states.AddRange(_statements.ExecuteCommand(puzzle));
            }
            return states;
        }

        internal override int GetLines()
        {
            return 1 + _statements.GetLines();
        }
    }
}
