using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace React_Frontend.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : Controller
    {
        [HttpGet("tallylines/{id}")]
        public string TallyEveryoneNumberOfLinesSolvedLevelsOf(int id)
        {
            Dictionary<int, Dictionary<int, int>> totalTally = Api.TallyEveryoneNumberOfLinesSolvedLevelsOf(id);
            return JSON.Serialize(totalTally);
        }
        [HttpGet("shortestsolutions/{id}")]
        public string NumberOfLinesSolvedLevelsOf(int id)
        {
            Dictionary<int, int> solutions = Api.NumberOfLinesSolvedLevelsOf(id);
            return JSON.Serialize(solutions);
        }
        [HttpGet("tallyduration/{id}")]
        public string TallyEveryoneDurationSolvedLevelsOf(int id)
        {
            Dictionary<int, Dictionary<int, int>> totalTally = Api.TallyEveryoneDurationSolvedLevelsOf(id);
            return JSON.Serialize(totalTally);
        }
        [HttpGet("durationshortestsolutions/{id}")]
        public string DurationSolvedLevelsOf(int id)
        {
            Dictionary<int, int> solutions = Api.DurationSolvedLevelsOf(id);
            return JSON.Serialize(solutions);
        }
    }
}