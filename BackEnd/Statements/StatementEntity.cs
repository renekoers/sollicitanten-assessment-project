using System;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
	public class StatementEntity : Entity
	{
        public void Test(){
            While w = new While(ConditionParameter.TileBehind,ConditionValue.Button,true, new Statement[]{new SingleCommand(Command.Drop)});
            Object[] input = new Object[]{ConditionParameter.TileBehind,ConditionValue.Button,true, new Statement[]{new SingleCommand(Command.Drop)}};
            Type type = w.GetType();
            String typeAsString = type.ToString();
            Type type1 = Type.GetType(typeAsString);
            Statement S = (Statement) Activator.CreateInstance(type1,input);
        }
	}
}
