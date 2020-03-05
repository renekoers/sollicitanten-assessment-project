using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BackEnd;
using React_Frontend.Controllers;

namespace UnitTest
{
    [TestClass]
    public class CandidateTest
    {        
        [TestMethod]
        async public Task GetCandidateRejectsIfIdIsNotGivenTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(null);
            Assert.AreNotEqual(200, ((StatusCodeResult) result.Result).StatusCode);
        }
        
        [TestMethod]
        async public Task GetCandidateRejectsIfIdIsInvalidTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(id + "NOT");
            Assert.AreNotEqual(200, ((StatusCodeResult) result.Result).StatusCode);
        }
        
        [TestMethod]
        async public Task GetCandidateAcceptsIfIdIsValidTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(id);
            Assert.IsTrue(result.Result == null || ((StatusCodeResult) result.Result).StatusCode == 200);
        }
        
        [TestMethod]
        async public Task GetCandidateReturnsTheRightCandidateTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(id);
            CandidateEntity candidate = JSON.Deserialize<CandidateEntity>(result.Value);
            Assert.AreEqual(id, candidate.ID);
        }

        [TestMethod]
        async public Task CannotGetStatusOfAnNonExistingCandidateTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getStatus(id + "NOT");
            Assert.AreNotEqual(200, ((StatusCodeResult) result.Result).StatusCode);
        }

        [TestMethod]
        async public Task StatusNotStartedCandidateTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getStatus(id);
            var definition = new {started = false, finished = false};
            var status = JsonConvert.DeserializeAnonymousType(result.Value, definition);
            Assert.IsFalse(status.started);
            Assert.IsFalse(status.finished);
        }

        [TestMethod]
        async public Task StatusCandidateWithActiveSessionTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            ActionResult<string> result = await controller.getStatus(id);
            var definition = new {started = false, finished = false};
            var status = JsonConvert.DeserializeAnonymousType(result.Value, definition);
            Assert.IsTrue(status.started);
            Assert.IsFalse(status.finished);
        }

        [TestMethod]
        async public Task StatusFinishedCandidateTest()
        {
            IRepository repo = new TestDB();
            CandidateController controller = new CandidateController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.getStatus(id);
            var definition = new {started = false, finished = false};
            var status = JsonConvert.DeserializeAnonymousType(result.Value, definition);
            Assert.IsTrue(status.started);
            Assert.IsTrue(status.finished);
        }
    }
}
