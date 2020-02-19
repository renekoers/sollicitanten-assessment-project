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
		public LevelSession[] GameResults { get; set; }

		public CandidateEntity(string name)
		{
			this.Name = name;
		}
		internal static LevelSession[] newGameResults()
		{
			LevelSession[] GameResults = new LevelSession[Level.TotalLevels];
			for(int index = 0; index<Level.TotalLevels; index++){
				GameResults[index] = new LevelSession(index+1);
			}
			return GameResults;
		}
	}
}
