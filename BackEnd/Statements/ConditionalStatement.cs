
namespace BackEnd
{
    enum ConditionalStatementName
    {
        While, DoWhile, IfElse, Repeat
    }
    public abstract class ConditionalStatement : Statement
    {
		private protected ConditionParameter _parameter;
		private protected ConditionValue _value;
		private protected bool _isTrue;
		private protected StatementBlock _statements;
        public ConditionalStatement(ConditionParameter parameter, ConditionValue value, bool isTrue){
            this.Condition = new ConditionEntity(parameter,value,isTrue);
            this._parameter = parameter;
            this._value = value;
            this._isTrue = isTrue;
        }
        internal override void CompleteProperties()
        {
            if(_statements == null || _statements._statements.Length == 0) // If the statementblock is not set correctly.
            {
                setConditions();
                foreach(Statement statement in Code)
                {
                    statement.CompleteProperties();
                }
                _statements = new StatementBlock(Code);
            }
        }
        internal void setConditions()
        {
                if(ConditionParameter.TryParse(Command, out ConditionParameter parameter))
                {
                    _parameter = parameter;
                }
                if(ConditionValue.TryParse(Command, out ConditionValue value))
                {
                    _value = value;
                }
                _isTrue = Condition.IsTrue;
        }
    }
}
