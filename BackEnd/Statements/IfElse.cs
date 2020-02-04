using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class IfElse : ConditionalStatement
    {
        private readonly ConditionParameter _parameter;
        private readonly ConditionValue _value;

        private List<Statement> _tempTrue;

        private List<Statement> _tempFalse;
        private readonly bool _isTrue;
        private  StatementBlock _statementsTrue;
        private  StatementBlock _statementsFalse;
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
            this._tempFalse = new List<Statement>();
            this._tempTrue = new List<Statement>();
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

        public void AddTrueStatement(Statement s){
            this._tempTrue.Add(s);
            this.ConvertStatementBlock();
        }

        public void AddFalseStatement(Statement s){
            this._tempFalse.Add(s);
            this.ConvertStatementBlock();
        }

        public void ConvertStatementBlock(){
            this._statementsFalse = new StatementBlock(this._tempFalse.ToArray());
            this._statementsTrue = new StatementBlock(this._tempTrue.ToArray());
        }
    }
}
