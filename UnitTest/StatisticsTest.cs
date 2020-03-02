using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackEnd;
using React_Frontend.Controllers;

namespace UnitTest
{
    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public async Task MakeStatisticsOfCandidateRejectsIfIdIsIncorrect()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.MakeStatisticsCandidate(id + "NOT");
            Assert.AreNotEqual(((ObjectResult) result.Result).StatusCode, 200);
        }
        
        [TestMethod]
        public async Task MakeStatisticsOfCandidateRejectsIfCandidateIsNotFinished()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            string id = await repo.AddCandidate("Test");
            await (new GameSessionController(repo)).StartSession(id);
            ActionResult<string> result = await controller.MakeStatisticsCandidate(id);
            Assert.AreNotEqual(((ObjectResult) result.Result).StatusCode, 200);
        }
        
        [TestMethod]
        public async Task MakeStatisticsOfCandidateAcceptIfCandidateIsFinished()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.MakeStatisticsCandidate(id);
            Assert.AreEqual(((ObjectResult) result.Result).StatusCode, 200);
        }

        [TestMethod]
        public async Task MakeStatisticsCandidateGivesOnlyStatisticsOfSolvedLevelsTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            // WIP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            await gameSessionController.EndSession(id);
        }
    }
}
