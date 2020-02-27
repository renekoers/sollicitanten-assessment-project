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
		async public Task<ActionResult<string>> PostStatements([FromHeader(Name="Authorization")] string sessionID, int levelNumber, [FromBody] JsonElement statementTreeJson)
		{
			IEnumerable<Statement> statements = Api.ParseStatementTreeJson(statementTreeJson);
			LevelSolution solution = await Api.SubmitSolution(sessionID, levelNumber, statements.ToArray());
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
