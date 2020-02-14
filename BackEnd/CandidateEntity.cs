using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Entities.Core;
using MongoDB.Entities;

namespace BackEnd
{
	[Name("candidates")]
	public class CandidateEntity : Entity
	{
		public string Name { get; protected set; }
		public DateTime started { get; protected set; }
		public DateTime finished { get; protected set; }
		public GameSession GameResults { get; set; }

		public CandidateEntity(string name)
		{
			this.Name = name;

		}
	}
}
