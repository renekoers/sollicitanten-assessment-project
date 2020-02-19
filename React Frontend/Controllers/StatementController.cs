using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/statement")]
	public class StatementController : Controller
	{
		[HttpPost("{levelId}")]
		async public Task<ActionResult<string>> PostStatements(int levelId, [FromBody] JsonElement statementTreeJson)
		{
			string sessionID = Request.Headers["Authorization"];
			IEnumerable<Statement> statements = Api.ParseStatementTreeJson(statementTreeJson);
			LevelSolution solution = await Api.SubmitSolution(sessionID, levelId, statements.ToArray());
			if(solution != null)
			{
				return JSON.Serialize(solution);
			} else
			{
				return BadRequest();
			}
		}
	}
}
