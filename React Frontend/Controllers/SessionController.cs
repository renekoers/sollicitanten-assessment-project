﻿using System;
using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        [HttpPost("remainingtime")]
        public long GetRemainingTime([FromBody]string ID)
        {
            int sessionID = int.Parse(Request.Headers["Authorization"]);
            GameSession session  = Api.GetSession(sessionID);
            return Math.Max(0, 1200000L - session.CurrentDuration); //20 minutes in milliseconds
        }

        [HttpGet("retrieveLevel")]
        public string GetLevel(string levelNumber)
        {
            int level = int.Parse(levelNumber);
            int sessionID = int.Parse(Request.Headers["Authorization"]);
            return JSON.Serialize(Api.StartLevelSession(sessionID, level));
        }
    }
}
