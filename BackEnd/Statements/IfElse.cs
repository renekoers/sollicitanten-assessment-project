﻿using System.Collections.Generic;

namespace BackEnd
{
    public class IfElse : ConditionalStatement
    {

        private List<Statement> _tempTrue;

        private List<Statement> _tempFalse;
        private  StatementBlock _statementsTrue;
        private  StatementBlock _statementsFalse;
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue)
            : this(parameter, value, isTrue, statementsTrue, new Statement[0]) { }
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue, Statement[] statementsFalse)
            : this(parameter, value, isTrue, new StatementBlock(statementsTrue), new StatementBlock(statementsFalse)) { }

        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statementsTrue, StatementBlock statementsFalse) : base(parameter, value, isTrue)
        {
            this._statementsTrue = statementsTrue;
            this._statementsFalse = statementsFalse;
            this._tempFalse = new List<Statement>();
            this._tempTrue = new List<Statement>();
            List<Statement> totalStatements = new List<Statement>(statementsTrue._statements);
            totalStatements.Add(new Else(statementsFalse));
            this.Code = totalStatements.ToArray();
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = null;
            if (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue)
            {
                states = _statementsTrue.ExecuteCommand(puzzle);
                if(_statementsTrue.IsInfiniteLoop)
                {
                    this.IsInfiniteLoop = true;
                }
            }
            else
            {
                states = _statementsFalse.ExecuteCommand(puzzle);
                if(_statementsFalse.IsInfiniteLoop)
                {
                    this.IsInfiniteLoop = true;
                }
            }
            return states;
        }

        internal override int GetLines()
        {
            return 1 + _statementsTrue.GetLines() + _statementsFalse.GetLines();
        }

        public void ConvertStatementBlock(){
            this._statementsFalse = new StatementBlock(this._tempFalse.ToArray());
            this._statementsTrue = new StatementBlock(this._tempTrue.ToArray());
        }
        internal class Else : Statement
        {
            private StatementBlock _statementsFalse;
            internal Else(StatementBlock statementsFalse)
            {
                this._statementsFalse = statementsFalse;
            }
            internal override List<State> ExecuteCommand(Puzzle puzzle)
            {
                throw new System.Exception("Invalid for Else.");
            }

            internal override int GetLines()
            {
                throw new System.Exception("Invalid for Else.");
            }
        }
    }
}
