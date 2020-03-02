using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
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
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        
        [TestMethod]
        public async Task MakeStatisticsOfCandidateRejectsIfCandidateIsNotFinished()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            string id = await repo.AddCandidate("Test");
            await (new GameSessionController(repo)).StartSession(id);
            ActionResult<string> result = await controller.MakeStatisticsCandidate(id);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
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
            Assert.IsTrue(result.Result == null || ((StatusCodeResult) result.Result).StatusCode == 200);
        }

        [TestMethod]
        public async Task MakeStatisticsCandidateGivesOnlyStatisticsOfSolvedLevelsTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            StatementController statementController = new StatementController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await InsertAttempt(levelSessionController, statementController, id, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await InsertAttempt(levelSessionController, statementController, id, 2, new Statement[]{new SingleCommand(Command.RotateLeft)});
            await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.MakeStatisticsCandidate(id);
            Dictionary<int,Dictionary<string,int>> statistics = JSON.Deserialize<Dictionary<int,Dictionary<string,int>>>(result.Value);
            Assert.AreEqual(1, statistics.Count);
            Assert.IsTrue(statistics.ContainsKey(1));
        }

        [TestMethod]
        public async Task MakeStatisticsCandidateGivesStatisticsOfAllSolvedLevelsTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            StatementController statementController = new StatementController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await InsertAttempt(levelSessionController, statementController, id, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await InsertAttempt(levelSessionController, statementController, id, 3, MockDataStatistics.GetAnswerLevel3WhileIf());
            await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.MakeStatisticsCandidate(id);
            Dictionary<int,Dictionary<string,int>> statistics = JSON.Deserialize<Dictionary<int,Dictionary<string,int>>>(result.Value);
            Assert.AreEqual(2, statistics.Count);
            Assert.IsTrue(statistics.ContainsKey(1));
            Assert.IsTrue(statistics.ContainsKey(3));
        }

        [TestMethod]
        public async Task MakeStatisticsCandidateGivesSameStatisticsForEverySolvedLevelTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            StatementController statementController = new StatementController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await InsertAttempt(levelSessionController, statementController, id, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await InsertAttempt(levelSessionController, statementController, id, 3, MockDataStatistics.GetAnswerLevel3WhileIf());
            await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.MakeStatisticsCandidate(id);
            Dictionary<int,Dictionary<string,int>> statistics = JSON.Deserialize<Dictionary<int,Dictionary<string,int>>>(result.Value);
            statistics.TryGetValue(1, out Dictionary<string,int> statisticsLevel1);
            statistics.TryGetValue(1, out Dictionary<string,int> statisticsLevel3);
            CollectionAssert.AreEqual(statisticsLevel1.Keys, statisticsLevel3.Keys);
        }

        private async Task InsertAttempt(LevelSessionController levelSessionController, StatementController statementController, string id, int levelNumber, Statement[] statements)
        {
            await levelSessionController.StartLevel(id,levelNumber.ToString());
            await statementController.SaveAttempt(id, levelNumber, statements);
            await levelSessionController.StopLevel(id, levelNumber.ToString());
        }
    }
}
