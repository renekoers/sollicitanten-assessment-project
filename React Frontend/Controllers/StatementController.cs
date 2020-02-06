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
            int sessionID = Int32.Parse(input[0]);
            int level = Int32.Parse(input[1]);
            string[] statements = new string[input.Length - 2];
            for (int i = 0; i < statements.Length; i++)
            {
                statements[i] = input[i+2];
            }
            LevelSolution lvl = Api.ConvertAndSubmit(sessionID, level, statements);
            return JSON.Serialize(lvl);
        }
    }
}
