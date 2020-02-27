// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using BackEnd;
// using React_Frontend.Controllers;

// namespace UnitTest
// {
//     [TestClass]
//     public class GameSessionTest
//     {
//         [TestMethod]
//         public async Task ControllerAcceptsValidIDTest()
//         {
//             IRepository repo = new TestDB();
//             GameSessionController controller = new GameSessionController(repo);
//             string id = await repo.AddCandidate("Test");
//             ActionResult result = await controller.StartSession(id);
//             Assert.IsTrue(result is OkResult);
//         }
//     }
// }
