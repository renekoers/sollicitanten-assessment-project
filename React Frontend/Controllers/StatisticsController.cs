using System;
using System.Collections.Generic;
using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/statistics"), Authorize]
	public class StatisticsController : Controller
	{
		[HttpGet("newFinished")]
		async public Task<ActionResult<string>> GetNewFinished(string time)
		{
			string timeReceivedRequest = DateTime.UtcNow.ToString();
			if(time==null){
				ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
				time = identity.FindFirst("Time").Value;
			}
			DateTime givenTime = DateTime.Parse(time);
			List<string> newFinishedIDs = await Api.GetFinishedIDsAfterTime(givenTime);
			if(newFinishedIDs.Count > 0){
				return JSON.Serialize(new {IDs = newFinishedIDs , time = timeReceivedRequest});
			} else {
				return NotFound(timeReceivedRequest);
			}
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
		[HttpGet("candidate")]
		public string GetDataCandidate(int id)
		{
			Dictionary<int,Dictionary<string,int>> data = Api.StatisticsCandidate(id);
			return JSON.Serialize(data);
		}
		[HttpGet("tally")]
		public string GetDataTally()
		{
			Dictionary<int,Dictionary<string, Dictionary<int, int>>> data = Api.StatisticsEveryone();
			return JSON.Serialize(data);
		}
        [HttpGet("unsolved")]
        public string GetDataUnsolved()
        {
            Dictionary<int,int> amountUnsolvedPerLevel = Api.NumberOfCandidatesNotSolvedPerLevel();
            return JSON.Serialize(amountUnsolvedPerLevel);
        }
	}
}