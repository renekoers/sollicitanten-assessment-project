using System;
using System.Collections.Generic;
using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/statistics"), Authorize]
	public class StatisticsController : Controller
	{
		[HttpGet("newFinished")]
		public ActionResult<List<string>> GetNewFinished(long time)
		{
			List<string> newFinishedIDs = Api.GetFinishedIDsAfterEpochTime(time);
			return newFinishedIDs;
		}
		[HttpGet("nextFinished")]
		public ActionResult<int> GetNextFinished(int ID)
		{
			int? nextID = Api.GetNextIDWhichIsFinished(ID);
			if (nextID == null)
			{
				return NotFound();
			}
			else
			{
				return nextID.Value;
			}
		}
		[HttpGet("previousFinished")]
		public ActionResult<int> GetPreviousFinished(int ID)
		{
			int? previousID = Api.GetPreviousIDWhichIsFinished(ID);
			if (previousID == null)
			{
				return NotFound();
			}
			else
			{
				return previousID.Value;
			}
		}
		[HttpGet("lastFinished")]
		public ActionResult<int> GetLastFinished()
		{
			int? lastID = Api.GetLastIDWhichIsFinished();
			if (lastID == null)
			{
				return NotFound();
			}
			else
			{
				return lastID.Value;
			}
		}
		[HttpGet("tallylines")]
		public string TallyEveryoneNumberOfLinesSolvedLevelsOf(string id)
		{
			string sessionID = id;
			Dictionary<int, Dictionary<int, int>> totalTally = Api.TallyEveryoneNumberOfLinesSolvedLevelsOf(sessionID);
			return JSON.Serialize(totalTally);
		}
		[HttpGet("shortestsolutions")]
		public string NumberOfLinesSolvedLevelsOf(string id)
		{
			string sessionID = id;
			Dictionary<int, int> solutions = Api.NumberOfLinesSolvedLevelsOf(sessionID);
			return JSON.Serialize(solutions);
		}
	}
}