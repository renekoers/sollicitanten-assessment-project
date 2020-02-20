using System;
using System.Collections.Generic;

namespace BackEnd
{
	public class While : ConditionalStatement
	{
		private List<Statement> _tempStatements;
		public While(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statements)
			: this(parameter, value, isTrue, new StatementBlock(statements)) { }
		public While(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statements) : base(parameter, value, isTrue)
		{
			this._statements = statements;
			this._tempStatements = new List<Statement>();
            this.Code = statements._statements;
		}
        // public void Test(){
		// 	if(_isTrue){
		// 		return;
		// 	}
        //     Object[] input = new Object[]{_parameter,_value,false, _statements};
        //     Type type = this.GetType();
        //     String typeAsString = type.ToString();
        //     Type type1 = Type.GetType(typeAsString);
        //     While S = (While) Activator.CreateInstance(type1,input);
		// 	System.Console.WriteLine("NEEEEEEEWWWW.......... ");
		// 	System.Console.WriteLine("Type: " + S.GetType().ToString());
		// 	System.Console.WriteLine("Parameter: " + S._parameter.ToString());
		// 	System.Console.WriteLine("Value: " + S._value.ToString());
		// 	System.Console.WriteLine("IsTrue: " + S._isTrue.ToString());
		// 	System.Console.WriteLine("Number of lines: " + S._statements.GetLines());
        // }
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

		private void ConvertStatements()
		{
			this._statements = new StatementBlock(this._tempStatements.ToArray());
		}
	}
}
