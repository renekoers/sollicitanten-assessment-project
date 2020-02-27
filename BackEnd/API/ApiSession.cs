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
        /// Level session methods.
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
    }
}