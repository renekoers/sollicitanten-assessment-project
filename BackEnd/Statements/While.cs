using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Statements
{
    class While : ConditionalStatement
    {
        private ConditionParameter _parameter;
        private ConditionValue _value;
        private bool _isTrue;
        private Statement[] _statements;
        public While(ConditionParameter parameter, ConditionValue value, bool IsTrue, Statement[] statements)
        {
            this._parameter = parameter;
            this._value = value;
            this._isTrue = IsTrue;
            this._statements = statements;
        }
        public override List<string[][]> ExecuteCommand(Puzzle puzzle, ICharacter character)
        {
            List<string[][]> states = new List<string[][]>();
            while(character.CheckCondition(_parameter, _value) == _isTrue)
            {
                foreach(Statement statement in _statements)
                {
                    states.AddRange(statement.ExecuteCommand(puzzle, character));
                }
            }
        }

        public override int GetLines()
        {
            throw new NotImplementedException();
        }
    }
}
