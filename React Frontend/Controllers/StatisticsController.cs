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
        public ActionResult<List<int>> GetNewFinished(long time)
        {
            List<int> newFinishedIDs = Api.GetNewFinishedIDs(time);
            return newFinishedIDs;
        }
        [HttpGet("nextFinished")]
        public ActionResult<int> GetNextFinished(int ID)
        {
            int? nextID = Api.GetNextFinishedID(ID);
            if(nextID==null){
                return NotFound();
            } else {
                return nextID.Value;
            }
        }
        [HttpGet("previousFinished")]
        public ActionResult<int> GetPreviousFinished(int ID)
        {
            int? previousID = Api.GetPreviousFinishedID(ID);
            if(previousID==null){
                return NotFound();
            } else {
                return previousID.Value;
            }
        }
		[HttpGet("lastFinished")]
		public ActionResult<int> GetLastFinished()
		{
			int? lastID =  Api.GetLastFinishedID();
            if(lastID==null){
                return NotFound();
            } else {
                return lastID.Value;
            }
		}
		[HttpGet("tallylines")]
		public string TallyEveryoneNumberOfLinesSolvedLevelsOf(int id)
		{
			Dictionary<int, Dictionary<int, int>> totalTally = Api.TallyEveryoneNumberOfLinesSolvedLevelsOf(id);
			return JSON.Serialize(totalTally);
		}
		[HttpGet("candidatelines")]
		public string NumberOfLinesSolvedLevelsOf(int id)
		{
			Dictionary<int, int> solutions = Api.NumberOfLinesSolvedLevelsOf(id);
			return JSON.Serialize(solutions);
		}
		[HttpGet("tallyduration")]
		public string TallyEveryoneDurationSolvedLevelsOf(int id)
		{
			Dictionary<int, Dictionary<int, int>> totalTally = Api.TallyEveryoneDurationSolvedLevelsOf(id);
			return JSON.Serialize(totalTally);
		}
		[HttpGet("candidateduration")]
		public string DurationSolvedLevelsOf(int id)
		{
			Dictionary<int, int> solutions = Api.DurationSolvedLevelsOf(id);
			return JSON.Serialize(solutions);
		}
		[HttpGet("candidate")]
		public string GetDataCandidate(int id)
		{
			Dictionary<string,Dictionary<int,int>> data = Api.DataSolvedLevelsOf(id);
			return JSON.Serialize(data);
		}
		[HttpGet("tally")]
		public string GetDataTally(int id)
		{
			Dictionary<string,Dictionary<int, Dictionary<int, int>>> data = Api.TallyEveryoneDataSolvedLevelsOf(id);
			return JSON.Serialize(data);
		}
	}
}