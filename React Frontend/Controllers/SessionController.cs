using System;
using BackEnd;
using Microsoft.AspNetCore.Mvc;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/session")]
	public class SessionController : Controller
	{
		[HttpGet("candidate")]
		public ActionResult<string> getCandidate()
		{
			Candidate candidate = Api.GetCandidate();
			if (candidate != null)
			{
				return JSON.Serialize(candidate);
			}
			else
			{
				return NotFound();
			}

		}
		[HttpGet("candidate/{id}")]
		public ActionResult<string> getCandidate(string id)
		{
			if(int.TryParse(id, out int sessionID))
			{
				Candidate candidate = Api.GetCandidate(sessionID);
				if (candidate != null)
				{
					return JSON.Serialize(candidate);
				}
				else
				{
					return NotFound();
				}
			} else {
				return BadRequest();
			}

		}
		[HttpGet("startsession")]
		public ActionResult StartSession()
		{
			if(int.TryParse(Request.Headers["Authorization"], out int sessionID) && Api.StartSession(sessionID))
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
		public ActionResult<bool> IsSessionValid()
		{
			if(int.TryParse(Request.Headers["Authorization"], out int sessionID))
			{
				return Api.IsUnstarted(sessionID);
			} else {
				return BadRequest();
			}
		}
		[HttpGet("isStarted")]
		public ActionResult<bool> isStarted()
		{
			if(int.TryParse(Request.Headers["Authorization"], out int sessionID))
			{
				return Api.IsStarted(sessionID);
			} else {
				return BadRequest();
			}
		}
		[HttpGet("levelIsSolved/{levelNumber}")]
		public bool IsSolved(string levelNumber)
		{
			int level = int.Parse(levelNumber);
			int sessionID = int.Parse(Request.Headers["Authorization"]);
			return Api.LevelIsSolved(sessionID, level);
		}
		[HttpGet("retrieveLevel/{levelNumber}")]
		public string GetLevel(string levelNumber)
		{
			int level = int.Parse(levelNumber);
			int sessionID = int.Parse(Request.Headers["Authorization"]);
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
			int sessionID = int.Parse(Request.Headers["Authorization"]);
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
			int sessionID = int.Parse(Request.Headers["Authorization"]);
			Api.EndSession(sessionID);
			return Ok();

		}
		[HttpGet("getOverview")]
		public string GetOverview()
		{
			int sessionID = int.Parse(Request.Headers["Authorization"]);
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
			int sessionID = int.Parse(Request.Headers["Authorization"]);
			GameSession session = Api.GetSession(sessionID);
			if (session == null)
			{
				return BadRequest();
			}
			return Math.Max(0, 1200000L - session.CurrentDuration); //20 minutes in milliseconds
		}
	}
}
