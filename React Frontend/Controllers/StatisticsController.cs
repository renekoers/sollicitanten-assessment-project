﻿using System;
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
            List<int> newFinishedIDs = Api.GetFinishedIDsAfterEpochTime(time);
            return newFinishedIDs;
        }
        [HttpGet("nextFinished")]
        public ActionResult<int> GetNextFinished(int ID)
        {
            int? nextID = Api.GetNextIDWhichIsFinished(ID);
            if(nextID==null){
                return NotFound();
            } else {
                return nextID.Value;
            }
        }
        [HttpGet("previousFinished")]
        public ActionResult<int> GetPreviousFinished(int ID)
        {
            int? previousID = Api.GetPreviousIDWhichIsFinished(ID);
            if(previousID==null){
                return NotFound();
            } else {
                return previousID.Value;
            }
        }
		[HttpGet("lastFinished")]
		public ActionResult<int> GetLastFinished()
		{
			int? lastID =  Api.GetLastIDWhichIsFinished();
            if(lastID==null){
                return NotFound();
            } else {
                return lastID.Value;
            }
		}
		[HttpGet("candidate")]
		public ActionResult<string> GetDataCandidate(int id)
		{
			Dictionary<int,Dictionary<string,int>> data = Api.StatisticsCandidate(id);
            if(data!=null){
			    return JSON.Serialize(data);
            } else {
                return NotFound();
            }
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