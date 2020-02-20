using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		/// <summary>
		/// Obtain the session from a CandidateEntity. This session should at some point be stopped, and can be used to check the duration etc.
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static GameSession GetSession(string ID)
		{
			return Repository.GetSession(ID);
		}
		/// <summary>
		/// Start a new session for a new CandidateEntity.!-- This method is used in mockdata and tests!!!!
		/// </summary>
		/// <returns>The ID of the newly created CandidateEntity used to access their session</returns>
		[Obsolete("Use StartSession(string ID) instead")]
		public static string StartSession()
		{
			return Repository.CreateSession();
		}
		async public static Task<bool> StartSession(string ID)
		{
			return await Repository.StartSession(ID);
		}
		async public static Task<bool> EndSession(string ID)
		{
			if (await Database.EndSession(ID))
			{
				GameSession gameSession = GetSession(ID);
				gameSession.End();
				Repository.UpdateSession(ID, gameSession);
				return true;
			}
			else
			{
				return false;
			}
		}
        /// Level session methods.
		public static IState StartLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = new LevelSession(levelNumber);
			gameSession.AddLevel(levelSession);
			Repository.UpdateSession(ID, gameSession);

			return new State(new Puzzle(Level.Get(levelNumber)));
		}
        
		public static void PauseLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.Pause();
			Repository.UpdateSession(ID, gameSession);
		}
		public static IState ContinueLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.Restart();
			Repository.UpdateSession(ID, gameSession);
			return new State(new Puzzle(Level.Get(levelNumber)));
		}

		public static void EndLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.End();
			Repository.UpdateSession(ID, gameSession);
		}
        /// Check property of session methods.
		public static bool LevelHasBeenStarted(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			return gameSession.GetSession(levelNumber) != null;
		}
		public static bool LevelIsSolved(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			if (levelSession == null)
			{
				return false;
			}
			return levelSession.Solved;
		}
        /// Submit a solution methods.
		public static IEnumerable<Statement> ParseStatementTreeJson(System.Text.Json.JsonElement statementTreeJson)
	   => StatementParser.ParseStatementTreeJson(statementTreeJson);
		/// <summary>
		/// Submit a new solution attempt
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="levelNumber"></param>
		/// <param name="statements"></param>
		/// <returns>A LevelSolution object which contains amongst other things a list of IState objects and whether the level was solved or not</returns>
		public static LevelSolution SubmitSolution(string ID, int levelNumber, Statement[] statements)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			LevelSolution solution = levelSession.Attempt(statements);
			Repository.UpdateSession(ID, gameSession);
			return solution;
		}

        /// Total methods.
		public static int GetTotalLevelAmount()
		{
			int totalLevels = Level.TotalLevels;
			return totalLevels;
		}
		public static Overview GetOverview(string ID)
		{
			return new Overview(GetSession(ID));
		}
		async public static Task<TimeSpan> GetRemainingTime(string ID)
		{
			CandidateEntity candidate = await Database.GetCandidate(ID);
			if(candidate == null || candidate.started == new DateTime() || candidate.finished != new DateTime())
			{
				return TimeSpan.Zero;
			}
			TimeSpan duration = DateTime.UtcNow - candidate.started;
			TimeSpan maxTime = new TimeSpan(0,20,0);
			return duration < maxTime ? maxTime-duration : TimeSpan.Zero; 

		}
    }
}