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
		private IRepository _repo;
		public StatisticsController() : this(new MongoDataBase()){}
		public StatisticsController(IRepository repo) : base()
		{
			_repo = repo;
		}

		[HttpGet("newFinished")]
		async public Task<ActionResult<string>> GetNewFinished(string time)
		{
			string timeReceivedRequest = DateTime.UtcNow.ToString(); // The time of receiving will be send in the response
			if(time==null){ // If no time is given, then this method will get the time from the token
				ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
				time = identity.FindFirst("Time").Value;
			}
			try{
				DateTime givenTime = DateTime.Parse(time);
			}
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
		async public Task<string> GetDataCandidate(string ID)
		{
			Dictionary<int, Dictionary<string, int>> data = await Api.StatisticsCandidate(ID);
			return JSON.Serialize(data);
		}
		[HttpGet("tally")]
		async public Task<string> GetDataTally()
		{
			Dictionary<int, Dictionary<string, Dictionary<int, int>>> data = await Api.StatisticsEveryone();
			return JSON.Serialize(data);
		}
		[HttpGet("unsolved")]
		async public Task<string> GetDataUnsolved()
		{
			Dictionary<int, int> amountUnsolvedPerLevel = await Api.NumberOfCandidatesNotSolvedPerLevel();
			return JSON.Serialize(amountUnsolvedPerLevel);
		}
	}
}