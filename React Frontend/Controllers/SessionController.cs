using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/session")]
	public class SessionController : Controller
	{

		[HttpGet("levelIsSolved/{levelNumber}")]
		async public Task<bool> IsSolved([FromHeader(Name="Authorization")] string sessionID, string levelNumber)
		{
			int level = int.Parse(levelNumber);
			return await Api.LevelIsSolved(sessionID, level);
		}
		[HttpPost("pauseLevel")]
		async public Task<ActionResult> PauseLevel([FromHeader(Name="Authorization")] string sessionID, [FromBody]object levelNumber)
		{
			int level = int.Parse(levelNumber.ToString());
			if(await Api.StopLevelSession(sessionID,level))
			{
				return Ok();
			} else {
				return BadRequest();
			}
		}
	}
}
