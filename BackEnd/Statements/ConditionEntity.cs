using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
    public class ConditionEntity : Entity
    {
		public string Parameter;
		public string Value;
		public bool IsTrue;
        public ConditionEntity(ConditionParameter parameter, ConditionValue value, bool isTrue){
            this.Parameter = parameter.ToString();
            this.Value = value.ToString();
            this.IsTrue = isTrue;
        }
    }
}
