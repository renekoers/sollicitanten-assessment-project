using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Entities.Core;

namespace BackEnd
{
    public class LevelSessionEntity : Entity
    {
        public int LevelNumber { get; protected set; }
		public DateTime StartTime { get; protected set; }
		public DateTime EndTime { get; protected set; }
		/// <summary>
		/// The duration of the session once it has been stopped or paused
		/// </summary>
		public long TotalDuration { get; protected set; }
		/// <summary>
		/// The current duration of the session while the session is still in progress
		/// </summary>
		public long CurrentDuration
		{
			get
			{
				if (InProgress)
				{
					return TotalDuration + (long) Math.Ceiling((DateTime.UtcNow - StartTime).TotalSeconds);
				}
				else
				{
					return TotalDuration;
				}
			}
		}
		public bool InProgress { get; private set; }
        protected List<LevelSolution> Solutions = new List<LevelSolution>();
        public int NumberOfAttempts => Solutions.Count;
        public int NumberOfAttemptsForFirstSolved => Solutions.FindIndex(s => s.Solved) + 1;
        public bool Solved => Solutions.Any(s => s.Solved);

        public LevelSessionEntity(int levelNumber)
        {
            LevelNumber = levelNumber;
			StartTime = DateTime.UtcNow;
			InProgress = true;
			TotalDuration = 0;
        }
		public void Pause()
		{
			End();
		}

		public void Restart()
		{
			if (!InProgress)
			{
				StartTime = DateTime.UtcNow;
				InProgress = true;
			}
		}
		public void End()
		{
			if (InProgress)
			{
				InProgress = false;
				EndTime = DateTime.UtcNow;
				TotalDuration += (long) Math.Ceiling((EndTime - StartTime).TotalSeconds);
			}
		}

        public LevelSolution FindFirstSolution()
        {
            return Solutions.Find(s => s.Solved);
        }
        public LevelSolution GetLeastLinesOfCodeSolution()
        {
            return Util.Min(Solutions.FindAll(s => s.Solved), s => s.Lines);
        }
        public LevelSolution GetLeastNumberOfStatesSolution()
        {
            return Util.Min(Solutions.FindAll(s => s.Solved), s => s.NumberOfStates);
        }
        public LevelSolution Attempt(Statement[] statements)
        {
            if (InProgress)
            {
                LevelSolution solution = new LevelSolution(LevelNumber, new StatementBlock(statements), CurrentDuration);
                Solutions.Add(solution);
                return solution;
            }
            else
            {
                throw new InvalidOperationException("Level session has already ended.");
            }

        }
        public static int GetLines(LevelSessionEntity session) => session.GetLeastLinesOfCodeSolution().Lines;
        public static Func<LevelSessionEntity,int> GetDurationPerPeriod(int period) => (session => GetDuration(session)/period*period);
        public static int GetDuration(LevelSessionEntity session) => (int)session.GetLeastLinesOfCodeSolution().Duration/1000;
    }
}
