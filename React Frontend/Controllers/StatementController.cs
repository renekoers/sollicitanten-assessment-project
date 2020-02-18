using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/statement")]
	public class StatementController : Controller
	{
		[HttpPost("{levelId}")]
		public string PostStatements(int levelId, [FromBody] JsonElement statementTreeJson)
		{
			string sessionID = Request.Headers["Authorization"];
			IEnumerable<Statement> statements = Api.ParseStatementTreeJson(statementTreeJson);
			return JSON.Serialize(Api.SubmitSolution(sessionID, levelId, statements.ToArray()));
		}
	}
}
