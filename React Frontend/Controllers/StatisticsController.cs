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
        /// <summary>
        /// This method needs to get the authorization of the request. This will be a token to validate HR!!!!!!!!
        /// </summary>
        /// <returns></returns>
		public Dictionary<string, Dictionary<string, int>> TallyEveryoneNumberOfLinesSolvedLevelsOf()
		{
            Console.WriteLine("This method needs to get the authorization of the request. This will be a token to validate HR!!!!!!!!");
			int sessionID = int.Parse(Request.Headers["Authorization"]);
			Dictionary<int, Dictionary<int, int>> totalTally = Api.TallyEveryoneNumberOfLinesSolvedLevelsOf(sessionID);
			Dictionary<string, Dictionary<string, int>> stringTotalTally = new Dictionary<string, Dictionary<string, int>>();
			foreach (KeyValuePair<int, Dictionary<int, int>> pair in totalTally)
			{
				Dictionary<string, int> stringPair = new Dictionary<string, int>();
				foreach (KeyValuePair<int, int> subPair in pair.Value)
				{
					stringPair.Add(subPair.Key.ToString(), subPair.Value);
				}
				stringTotalTally.Add(pair.Key.ToString(), stringPair);
			}
			return stringTotalTally;
		}
		[HttpGet("shortestsolutions")]
        /// <summary>
        /// This method needs to get the authorization of the request. This will be a token to validate HR!!!!!!!!
        /// </summary>
        /// <returns></returns>
		public Dictionary<string, int> NumberOfLinesSolvedLevelsOf()
		{
            Console.WriteLine("This method needs to get the authorization of the request. This will be a token to validate HR!!!!!!!!");
			int sessionID = int.Parse(Request.Headers["Authorization"]);
			Dictionary<int, int> solutions = Api.NumberOfLinesSolvedLevelsOf(sessionID);
			Dictionary<string, int> solutionsString = new Dictionary<string, int>();
			foreach (KeyValuePair<int, int> pair in solutions)
			{
				solutionsString.Add(pair.Key.ToString(), pair.Value);
			}
			return solutionsString;
		}
	}
}