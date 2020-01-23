using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Statements
{
    class IfElse : ConditionalStatement
    {
        private ConditionParameter _parameter;
        private ConditionValue _value;
        private bool _isTrue;
        private Statement[] _statementsTrue;
        private Statement[] _statementsFalse;
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue) : this(parameter, value, isTrue, statementsTrue, new Statement[0])
        {
            
        }
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue, Statement[] statementsFalse)
        {
            this._parameter = parameter;
            this._value = value;
            this._isTrue = isTrue;
            this._statementsTrue = statementsTrue;
            this._statementsFalse = statementsFalse;
        }
        public override List<string[][]> ExecuteCommand(Puzzle puzzle, ICharacter character)
        {
            List<string[][]> states = new List<string[][]>();
            Statement[] statements;
            if (character.CheckCondition(_parameter, _value) == _isTrue)
            {
                statements = _statementsTrue;
            } else
            {
                statements = _statementsFalse;
            }
            foreach (Statement statement in statements)
            {
                states.AddRange(statement.ExecuteCommand(puzzle, character));
            }
            return states;
        }

        public override int GetLines()
        {
            int lines = 1;
            foreach (Statement statement in _statementsTrue)
            {
                lines += statement.GetLines();
            }
            foreach (Statement statement in _statementsFalse)
            {
                lines += statement.GetLines();
            }
            return lines;
        }
    }
}
