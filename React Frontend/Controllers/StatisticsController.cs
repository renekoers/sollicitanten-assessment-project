using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEnd;

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
		async public Task<ActionResult<string>> GetNextFinished(string ID)
		{
			string nextID = await Api.GetNextFinishedID(ID);
			if (nextID == null)
			{
				return NotFound();
			}
			else
			{
				return nextID;
			}
		}
		[HttpGet("previousFinished")]
		async public Task<ActionResult<string>> GetPreviousFinished(string ID)
		{
			string previousID = await Api.GetPreviousFinishedID(ID);
			if (previousID == null)
			{
				return NotFound();
			}
			else
			{
				return previousID;
			}
		}
		[HttpGet("lastFinished")]
		async public Task<ActionResult<string>> GetLastFinished()
		{
			string lastID = await Api.GetLastFinishedID();
			if (lastID == null)
			{
				return NotFound();
			}
			else
			{
				return lastID;
			}
		}
		[HttpGet("candidate")]
		public string GetDataCandidate(string ID)
		{
			Dictionary<int, Dictionary<string, int>> data = Api.StatisticsCandidate(ID);
			return JSON.Serialize(data);
		}
		[HttpGet("tally")]
		public string GetDataTally()
		{
			Dictionary<int, Dictionary<string, Dictionary<int, int>>> data = Api.StatisticsEveryone();
			return JSON.Serialize(data);
		}
		[HttpGet("unsolved")]
		public string GetDataUnsolved()
		{
			Dictionary<int, int> amountUnsolvedPerLevel = Api.NumberOfCandidatesNotSolvedPerLevel();
			return JSON.Serialize(amountUnsolvedPerLevel);
		}
	}
}