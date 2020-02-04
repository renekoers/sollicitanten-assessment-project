//Als deze nog in het project zit, is Jeroen dom geweest.

using System;
using BackEnd;
using Microsoft.AspNetCore.Mvc;

namespace React_Frontend.Controllers
{
    [ApiController]
    [Route("api/mock")]
    public class MockController : Controller
    {
        [HttpPost]
        public string GetMockData([FromBody]int id)
        {
            StatementBlock repeatBlock = new StatementBlock(new Statement[] {
                new SingleCommand(Command.RotateRight),
            });

            LevelSolution solution = Api.SubmitSolution(id, 1, new Statement[] {
                new Repeat(3, repeatBlock),
                new SingleCommand(Command.MoveForward),
            });

            return JSON.Serialize(solution);
        }
    }
}
