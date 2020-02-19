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
		[HttpGet("startsession")]
		async public Task<ActionResult> StartSession()
		{
			string sessionID = Request.Headers["Authorization"];
			if (await Api.StartSession(sessionID))
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

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
		[HttpGet("totalAmountLevels")]
		public int GetTotalAmountLevels()
		{
			return Api.GetTotalLevelAmount();
		}
		[HttpPost("endSession")]
		async public Task<StatusCodeResult> EndSession()
		{
			string sessionID = Request.Headers["Authorization"];
			if (await Api.EndSession(sessionID))
			{
				return Ok();
			}
			return Conflict();

		}
		[HttpGet("getOverview")]
		public string GetOverview()
		{
			string sessionID = Request.Headers["Authorization"];
			return JSON.Serialize(Api.GetOverview(sessionID));
		}

		/// <summary>
		/// Returns the remaining time the session has (starting at 20 minutes) in milliseconds.
		/// </summary>
		/// <param name="ID"> Session ID</param>
		/// <returns> Remaining time in milliseconds</returns>
		[HttpGet("remainingTime")]
		public ActionResult<long> GetRemainingTime()
		{
			string sessionID = Request.Headers["Authorization"];
			GameSession session = Api.GetSession(sessionID);
			if (session == null)
			{
				return BadRequest();
			}
			return Math.Max(0, 1200000L - session.CurrentDuration); //20 minutes in milliseconds
		}
	}
}
