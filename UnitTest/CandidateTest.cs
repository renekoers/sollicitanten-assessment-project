using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
            IRepository _repo = new TestDB();
            CandidateController controller = new CandidateController(_repo);
            await _repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(null);
            Assert.AreNotEqual(200, ((StatusCodeResult) result.Result).StatusCode);
        }
        
        [TestMethod]
        async public Task GetCandidateRejectsIfIdIsInvalidTest()
        {
            IRepository _repo = new TestDB();
            CandidateController controller = new CandidateController(_repo);
            string id = await _repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(id + "NOT");
            Assert.AreNotEqual(200, ((StatusCodeResult) result.Result).StatusCode);
        }
        
        [TestMethod]
        async public Task GetCandidateAcceptsIfIdIsValidTest()
        {
            IRepository _repo = new TestDB();
            CandidateController controller = new CandidateController(_repo);
            string id = await _repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(id);
            Assert.IsTrue(result.Result == null || ((StatusCodeResult) result.Result).StatusCode == 200);
        }
        
        [TestMethod]
        async public Task GetCandidateReturnsTheRightCandidateTest()
        {
            IRepository _repo = new TestDB();
            CandidateController controller = new CandidateController(_repo);
            string id = await _repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getCandidateName(id);
            CandidateEntity candidate = JSON.Deserialize<CandidateEntity>(result.Value);
            Assert.AreEqual(id, candidate.ID);
        }

        [TestMethod]
        async public Task CannotGetStatusOfAnNonExistingCandidateTest()
        {
            IRepository _repo = new TestDB();
            CandidateController controller = new CandidateController(_repo);
            string id = await _repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getStatus(id + "NOT");
            Assert.AreNotEqual(200, ((StatusCodeResult) result.Result).StatusCode);
        }

        [TestMethod]
        async public Task StatusNotStartedCandidateTest()
        {
            IRepository _repo = new TestDB();
            CandidateController controller = new CandidateController(_repo);
            string id = await _repo.AddCandidate("Test");
            ActionResult<string> result = await controller.getStatus(id);
            
        }
    }
}
