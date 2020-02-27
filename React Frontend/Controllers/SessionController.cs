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
		async public Task<bool> IsSolved(string levelNumber)
		{
			int level = int.Parse(levelNumber);
			string sessionID = Request.Headers["Authorization"];
			return await Api.LevelIsSolved(sessionID, level);
		}
		[HttpGet("retrieveLevel/{levelNumber}")]
		async public Task<ActionResult<string>> GetLevel(string levelNumber)
		{
			int level = int.Parse(levelNumber);
			string sessionID = Request.Headers["Authorization"];
			IState levelState = await Api.StartLevelSession(sessionID, level);
			if(levelState != null)
			{
				return JSON.Serialize(levelState);
			} else 
			{
				return BadRequest();
			}
		}
		[HttpPost("pauseLevel")]
		async public Task<ActionResult> PauseLevel([FromBody]object levelNumber)
		{
			int level = int.Parse(levelNumber.ToString());
			string sessionID = Request.Headers["Authorization"];
			if(await Api.StopLevelSession(sessionID,level))
			{
				return Ok();
			} else {
				return BadRequest();
			}
		}
	}
}
