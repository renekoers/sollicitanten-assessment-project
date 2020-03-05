using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
	[BsonKnownTypes(typeof(DoWhile), typeof(IfElse), typeof(Else), typeof(Repeat), typeof(While), typeof(SingleCommand))]
	public class Statement : Entity
	{
		[Ignore]
		public static uint MaxStates { get; } = 100;
		[Ignore]
		internal bool IsInfiniteLoop { get; set; } = false;
		public ConditionEntity Condition;
		public Statement[] Code;
		public string Command;
		internal virtual List<State> ExecuteCommand(Puzzle puzzle)
		{
			throw new Exception();
		}
		internal virtual int GetLines()
		{
			throw new Exception();
		}
		internal virtual void CompleteProperties()
		{
			throw new Exception();
		}
		///<summary>
		/// This method converts a statement that came from the database to its original type.
		///</summary>
		///<returns>Child of statement that is an instance of the current representation.</returns>
		internal Statement GetStatementAsOriginalType()
		{
			return null;
		}
	}
}
