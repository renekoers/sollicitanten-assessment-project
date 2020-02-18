using System.Collections.Generic;

namespace BackEnd
{
	public class While : ConditionalStatement
	{
		private readonly ConditionParameter _parameter;
		private readonly ConditionValue _value;
		private readonly bool _isTrue;
		private StatementBlock _statements;

		private List<Statement> _tempStatements;
		public While(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statements)
			: this(parameter, value, isTrue, new StatementBlock(statements)) { }
		public While(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statements)
		{
			this._parameter = parameter;
			this._value = value;
			this._isTrue = isTrue;
			this._statements = statements;
			this._tempStatements = new List<Statement>();
		}
		internal override List<State> ExecuteCommand(Puzzle puzzle)
		{
			List<State> states = new List<State>();
			List<State> oldStates = new List<State>();
			State newState = null;
			while (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue)
			{
				states.AddRange(_statements.ExecuteCommand(puzzle));
				newState = states.Count>0 ? states[states.Count - 1] : null;
				if (oldStates.Contains(newState) || newState==null)
				{
					this.IsInfiniteLoop = true;
					return states;
				}
				oldStates.Add(newState);
				if (_statements.IsInfiniteLoop)
				{
					this.IsInfiniteLoop = true;
					return states;
				}
				if (states.Count >= MaxStates)
				{
					return states;
				}
			}
			return states;
		}

		internal override int GetLines()
		{
			return 1 + _statements.GetLines();
		}

		public void AddStatement(Statement s)
		{
			this._tempStatements.Add(s);
			ConvertStatements();
		}

		private void ConvertStatements()
		{
			this._statements = new StatementBlock(this._tempStatements.ToArray());
		}
	}
}
