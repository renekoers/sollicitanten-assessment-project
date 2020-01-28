using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class DoWhile : ConditionalStatement
    {
        private readonly ConditionParameter _parameter;
        private readonly ConditionValue _value;
        private readonly bool _isTrue;
        private readonly Statement[] _statements;
        public DoWhile(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statements)
        {
            this._parameter = parameter;
            this._value = value;
            this._isTrue = isTrue;
            this._statements = statements;
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            do
            {
                foreach (Statement statement in _statements)
                {
                    states.AddRange(statement.ExecuteCommand(puzzle));
                }
            } while (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue);
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
