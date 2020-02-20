using System;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
	[Name("candidates")]
	public class CandidateEntity : Entity
	{
		public string Name { get; protected set; }
		public DateTime started { get; protected set; }
		public DateTime finished { get; protected set; }
		public Object GameResults { get; set; }

		public CandidateEntity(string name)
		{
			this.Name = name;

		}
	}
}
