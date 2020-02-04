using System;
using BackEnd;
using Microsoft.AspNetCore.Mvc;

namespace React_Frontend.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : Controller
    {
        // GET: api/session/startsession
        [HttpGet("startsession")]
        public int StartSession()
        {
            return Api.StartSession();
        }

        /// <summary>
        /// Returns the remaining time the session has (starting at 20 minutes) in milliseconds.
        /// </summary>
        /// <param name="ID"> Session ID</param>
        /// <returns> Remaining time in milliseconds</returns>
        [HttpGet("remainingtime")]
        public ActionResult<long> GetRemainingTime()
        {
            int sessionID = int.Parse(Request.Headers["Authorization"]);
            GameSession session = Api.GetSession(sessionID);
            if(session == null){
                return BadRequest();
            }
            return Math.Max(0, 1200000L - session.CurrentDuration); //20 minutes in milliseconds
        }

        [HttpGet("sessionValidation")]
        public Boolean IsSessionValid()
        {
            int sessionID = int.Parse(Request.Headers["Authorization"]);
            if (Api.GetSession(sessionID) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet("retrieveLevel")]
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
        [HttpGet("getOverview")]
        public string GetOverview()
        {
            int sessionID = int.Parse(Request.Headers["Authorization"]);
            return JSON.Serialize(Api.GetOverview(sessionID));
        }
    }
}
