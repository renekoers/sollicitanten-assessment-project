using System.Collections.Generic;

namespace BackEnd
{
    public class IfElse : ConditionalStatement
    {
        private  StatementBlock _statementsFalse;
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue)
            : this(parameter, value, isTrue, statementsTrue, new Statement[0]) { }
        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statementsTrue, Statement[] statementsFalse)
            : this(parameter, value, isTrue, new StatementBlock(statementsTrue), new StatementBlock(statementsFalse)) { }

        public IfElse(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statementsTrue, StatementBlock statementsFalse) : base(parameter, value, isTrue)
        {
            Statement lastStatement = statementsTrue._statements[statementsTrue._statements.Length-1];
            if(lastStatement.GetType() == typeof(Else))
            {
                List<Statement> statementsTrueToList = new List<Statement>(statementsTrue._statements);
                statementsFalse = new StatementBlock(lastStatement.Code);
                statementsTrueToList.RemoveAt(statementsTrueToList.Count-1);
                statementsTrue = new StatementBlock(statementsTrueToList.ToArray());
            }
            this._statements = statementsTrue;
            this._statementsFalse = statementsFalse;
            List<Statement> totalStatements = new List<Statement>(statementsTrue._statements);
            totalStatements.Add(new Else(statementsFalse));
            this.Code = totalStatements.ToArray();
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = null;
            if (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue)
            {
                states = _statements.ExecuteCommand(puzzle);
                if(_statements.IsInfiniteLoop)
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
            return 1 + _statements.GetLines() + _statementsFalse.GetLines();
        }
        internal override void CompleteProperties()
        {
            if(_statements == null || _statements._statements.Length == 0)
            {
                Statement lastStatement = Code[Code.Length-1];
                List<Statement> statementsTrueToList = new List<Statement>(Code);
                this._statementsFalse = new StatementBlock(lastStatement.Code);
                statementsTrueToList.RemoveAt(statementsTrueToList.Count-1);
                this._statements = new StatementBlock(statementsTrueToList.ToArray());
                setConditions();
                foreach(Statement statement in _statements._statements)
                {
                    statement.CompleteProperties();
                }
                foreach(Statement statement in _statementsFalse._statements)
                {
                    statement.CompleteProperties();
                }
            }
        }
    }
    public class Else : Statement
    {
        public Else(Statement[] statements) : this(new StatementBlock(statements)){}
        public Else(StatementBlock statementsFalse)
        {
            this.Code = statementsFalse._statements;
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
