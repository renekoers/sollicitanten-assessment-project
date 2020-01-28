using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class IfElse : ConditionalStatement
    {
        private readonly ConditionParameter _parameter;
        private readonly ConditionValue _value;
        private readonly bool _isTrue;
        private readonly StatementBlock _statementsTrue;
        private readonly StatementBlock _statementsFalse;
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue)
            : this(parameter, value, isTrue, statementsTrue, new Statement[0]) { }
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue, Statement[] statementsFalse)
            : this(parameter, value, isTrue, new StatementBlock(statementsTrue), new StatementBlock(statementsFalse)) { }

        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statementsTrue, StatementBlock statementsFalse)
        {
            this._parameter = parameter;
            this._value = value;
            this._isTrue = isTrue;
            this._statementsTrue = statementsTrue;
            this._statementsFalse = statementsFalse;
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            if (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue)
            {
                return _statementsTrue.ExecuteCommand(puzzle);
            }
            else
            {
                return _statementsFalse.ExecuteCommand(puzzle);
            }
        }

        internal override int GetLines()
        {
            return 1 + _statementsTrue.GetLines() + _statementsFalse.GetLines();
        }
    }
}
