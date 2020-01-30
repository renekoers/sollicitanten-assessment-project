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
        public Dictionary<string, Dictionary<string, int>> TallyEveryoneNumberOfLinesSolvedLevelsOf(int id)
        {
            Dictionary<int, Dictionary<int, int>> totalTally = Api.TallyEveryoneNumberOfLinesSolvedLevelsOf(id);
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
    }
}