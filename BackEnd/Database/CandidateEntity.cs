using System;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
	[Name("candidates")]
	public class CandidateEntity : Entity
	{
		private static TimeSpan maxDuration = new TimeSpan(0,20,0);
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
		public LevelSession GetLevelSession(int levelNumber)
		{
			return (levelNumber>0 && levelNumber-1 <= GameResults.Length) ? GameResults[levelNumber-1] : null;
		}
		public bool IsStarted() => started > new DateTime();
		public bool IsFinished() => finished > new DateTime();
		public bool HasTimeLeft() => IsStarted() && !IsFinished() && DateTime.UtcNow - started < maxDuration;
		public TimeSpan GetRemainingTime() => HasTimeLeft() ? maxDuration - (DateTime.UtcNow - started) : TimeSpan.Zero;
	}
}
