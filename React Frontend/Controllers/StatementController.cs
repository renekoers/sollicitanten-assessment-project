using System;
using BackEnd;
using Microsoft.AspNetCore.Mvc;

namespace React_Frontend.Controllers
{
    [ApiController]
    [Route("api/statement")]
    public class StatementController : Controller
    {
        // Post the found statements to the backend "api/statement/deliver"
        [HttpPost("deliver")]
        public string PostStatements([FromBody]string[] input)
        {
            int sessionID = Int32.Parse(Request.Headers["Authorization"]);
            int level = Int32.Parse(input[0]);
            string[] statements = new string[input.Length - 1];
            for (int i = 0; i < statements.Length; i++)
            {
                statements[i] = input[i+1];
            }
            LevelSolution lvl = Api.ConvertAndSubmit(sessionID, level, statements);
            return JSON.Serialize(lvl);
        }
    }
}
