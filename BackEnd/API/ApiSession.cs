using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
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
			return await Database.StartSession(ID);
		}
		async public static Task<bool> EndSession(string ID)
		{
			return await Database.EndSession(ID);
		}
        /// Level session methods.
		async public static Task<IState> StartLevelSession(string ID, int levelNumber)
		{
			return (await Database.StartLevel(ID, levelNumber)) ? new State(new Puzzle(Level.Get(levelNumber))) : null;
		}
		async public static Task<bool> StopLevelSession(string ID, int levelNumber)
		{
			return await Database.StopLevel(ID, levelNumber);
		}

		[Obsolete("Remove function after mock data is fixed!!")]
		public static void EndLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.End();
			Repository.UpdateSession(ID, gameSession);
		}
        /// Check property of session methods.
		async public static Task<bool> LevelIsSolved(string ID, int levelNumber)
		{
			LevelSession levelSession = await Database.GetLevelSession(ID, levelNumber);
			return levelSession != null && levelSession.Solved;
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
		async public static Task<LevelSolution> SubmitSolution(string ID, int levelNumber, Statement[] statements)
		{
			return await Database.SubmitSolution(ID,levelNumber,statements);
		}

        /// Total methods.
		public static int GetTotalLevelAmount()
		{
			int totalLevels = Level.TotalLevels;
			return totalLevels;
		}
		async public static Task<Overview> GetOverview(string ID)
		{
			LevelSession[] levelSessions = await Database.GetAllLevelSessions(ID);
			return new Overview(levelSessions);
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