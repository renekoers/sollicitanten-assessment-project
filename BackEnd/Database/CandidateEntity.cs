using System;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
	[Name("candidates")]
	public class CandidateEntity : Entity
	{
		public string Name { get; protected set; }
		public DateTime started { get; internal set; }
		public DateTime finished { get; internal set; }
		public LevelSessionEntity[] GameResults { get; internal set; }

		public CandidateEntity(string name)
		{
			this.Name = name;
			GameResults = new LevelSessionEntity[Level.TotalLevels];
		}
		public LevelSessionEntity GetLevelSession(int levelNumber) => GameResults[levelNumber-1];
	}
}
