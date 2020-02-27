using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
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