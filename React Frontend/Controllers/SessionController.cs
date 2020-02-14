using System;
using BackEnd;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/session")]
	public class SessionController : Controller
	{
		[HttpGet("candidate")]
		async public Task<ActionResult<string>> getCandidate()
		{
			CandidateEntity candidate = await Api.GetCandidate();
			if (candidate == null)
			{
				return NotFound();
			}
			else
			{
				return JSON.Serialize(candidate);
			}

		}
		[HttpGet("candidate/{id}")]
		async public Task<ActionResult<string>> getCandidateName(string id)
		{
			CandidateEntity candidate = await Api.GetCandidate(id);
			if (candidate == null)
			{
				return NotFound();
			}
			else
			{
				return JSON.Serialize(candidate);
			}

		}
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

		[HttpGet("startTutorialSession")]
		public StatusCodeResult StartTutorialSession()
		{
			Api.StartTutorialSession();
			return Ok();
		}

		[HttpGet("sessionIDValidation")]
		async public Task<bool> IsSessionValid()
		{
			string sessionID = Request.Headers["Authorization"];
			return await Api.IsUnstarted(sessionID);
		}
		[HttpGet("isStarted")]
		public bool isStarted()
		{
			string sessionID = Request.Headers["Authorization"];
			return Api.IsStarted(sessionID);
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
		public StatusCodeResult EndSession()
		{
			string sessionID = Request.Headers["Authorization"];
			Api.EndSession(sessionID);
			return Ok();

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
