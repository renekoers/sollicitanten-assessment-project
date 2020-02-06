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
        private readonly StatementBlock _statements;
        public DoWhile(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statements)
            : this(parameter, value, isTrue, new StatementBlock(statements)) { }
        public DoWhile(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statements)
        {
            this._parameter = parameter;
            this._value = value;
            this._isTrue = isTrue;
            this._statements = statements;
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            List<State> oldStates = new List<State>();
            State newState = null;
            do
            {
                states.AddRange(_statements.ExecuteCommand(puzzle));
                newState = states[states.Count-1];
                if(oldStates.Contains(newState))
                {
                    this.IsInfiniteLoop = true;
                    return states;
                }
                oldStates.Add(newState);
                if(_statements.IsInfiniteLoop)
                {
                    this.IsInfiniteLoop = true;
                    return states;
                }
            } while (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue);
            return states;
        }

        internal override int GetLines()
        {
            return 1 + _statements.GetLines();
        }
    }
}
