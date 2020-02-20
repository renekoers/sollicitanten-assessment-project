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
		public bool IsSolved(string levelNumber)
		{
			int level = int.Parse(levelNumber);
			string sessionID = Request.Headers["Authorization"];
			return Api.LevelIsSolved(sessionID, level);
		}
		[HttpGet("retrieveLevel/{levelNumber}")]
		public string GetLevel(string levelNumber)
		{
			int level = int.Parse(levelNumber);
			string sessionID = Request.Headers["Authorization"];
			if (Api.LevelHasBeenStarted(sessionID, level))
			{
				return JSON.Serialize(Api.ContinueLevelSession(sessionID, level));
			}
			else
			{
				return JSON.Serialize(Api.StartLevelSession(sessionID, level));
			}
		}
		[HttpPost("pauseLevel")]
		public StatusCodeResult PauseLevel([FromBody]object levelNumber)
		{
			int level = int.Parse(levelNumber.ToString());
			string sessionID = Request.Headers["Authorization"];
			Api.PauseLevelSession(sessionID, level);
			return Ok();
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
		async public Task<ActionResult<long>> GetRemainingTime()
		{
			string sessionID = Request.Headers["Authorization"];
			TimeSpan remainingTime = await Api.GetRemainingTime(sessionID);
			if(remainingTime != TimeSpan.Zero)
			{
				return (long) Math.Floor(remainingTime.TotalMilliseconds);
			}
			else
			{
				return new StatusCodeResult(410);
			}

		}
	}
}
