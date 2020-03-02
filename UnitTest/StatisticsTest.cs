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

        [TestMethod]
        public async Task MakeStatisticsEveryoneGivesDataForEveryLevel()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            StatementController statementController = new StatementController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.MakeStatisticsEveryone();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> statistics = JSON.Deserialize<Dictionary<int,Dictionary<string, Dictionary<int, int>>>>(result.Value);
            Assert.AreEqual(Level.TotalLevels, statistics.Count);
            Assert.IsTrue(statistics.ContainsKey(Level.TotalLevels));
        }

        [TestMethod]
        public async Task MakeStatisticsEveryoneOnlyAnalysesCandidatesThatAreFinished()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            StatementController statementController = new StatementController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await InsertAttempt(levelSessionController, statementController, id, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await gameSessionController.EndSession(id);
            string idNotFinished = await repo.AddCandidate("Test not finished");
            await gameSessionController.StartSession(idNotFinished);
            await InsertAttempt(levelSessionController, statementController, idNotFinished, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            ActionResult<string> result = await controller.MakeStatisticsEveryone();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> statistics = JSON.Deserialize<Dictionary<int,Dictionary<string, Dictionary<int, int>>>>(result.Value);
            // Checks if the statistics only have a value for the candidate that is finished.
            Dictionary<string, Dictionary<int, int>> levelStatistics = statistics.GetValueOrDefault(1);
            string nameFirstStatistic = levelStatistics.Keys.First();
            levelStatistics.TryGetValue(nameFirstStatistic, out Dictionary<int,int> dataFirstStatistic);
            Assert.AreEqual(1, dataFirstStatistic.Count);
            Assert.AreEqual(1, dataFirstStatistic.Values.First());
        }

        [TestMethod]
        public async Task MakeStatisticsEveryoneMakesBarsOfAllValuesTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            StatementController statementController = new StatementController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await InsertAttempt(levelSessionController, statementController, id, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await gameSessionController.EndSession(id);
            string id2 = await repo.AddCandidate("Test 2");
            await gameSessionController.StartSession(id2);
            await InsertAttempt(levelSessionController, statementController, id2, 1, MockDataStatistics.GetAnswerLevel1LongerWithLoop());
            await gameSessionController.EndSession(id2);
            string id3 = await repo.AddCandidate("Test 3");
            await gameSessionController.StartSession(id3);
            await InsertAttempt(levelSessionController, statementController, id3, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await gameSessionController.EndSession(id3);
            ActionResult<string> result = await controller.MakeStatisticsEveryone();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> statistics = JSON.Deserialize<Dictionary<int,Dictionary<string, Dictionary<int, int>>>>(result.Value);
            // Checks if the statistics only have a value for the candidate that is finished.
            Dictionary<string, Dictionary<int, int>> levelStatistics = statistics.GetValueOrDefault(1);
            levelStatistics.TryGetValue("Regels code kortste oplossing", out Dictionary<int,int> dataFirstStatistic);
            // We have 2 different values namely 8 and 10 lines.
            Assert.AreEqual(2, dataFirstStatistic.Count);
        }

        [TestMethod]
        public async Task MakeStatisticsEveryonePlacesTheRightAmountToTheValuesTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
            LevelSessionController levelSessionController = new LevelSessionController(repo);
            StatementController statementController = new StatementController(repo);
            string id = await repo.AddCandidate("Test");
            await gameSessionController.StartSession(id);
            await InsertAttempt(levelSessionController, statementController, id, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await gameSessionController.EndSession(id);
            string id2 = await repo.AddCandidate("Test 2");
            await gameSessionController.StartSession(id2);
            await InsertAttempt(levelSessionController, statementController, id2, 1, MockDataStatistics.GetAnswerLevel1LongerWithLoop());
            await gameSessionController.EndSession(id2);
            string id3 = await repo.AddCandidate("Test 3");
            await gameSessionController.StartSession(id3);
            await InsertAttempt(levelSessionController, statementController, id3, 1, MockDataStatistics.GetAnswerLevel1HardCoded());
            await gameSessionController.EndSession(id3);
            ActionResult<string> result = await controller.MakeStatisticsEveryone();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> statistics = JSON.Deserialize<Dictionary<int,Dictionary<string, Dictionary<int, int>>>>(result.Value);
            // Checks if the statistics only have a value for the candidate that is finished.
            Dictionary<string, Dictionary<int, int>> levelStatistics = statistics.GetValueOrDefault(1);
            levelStatistics.TryGetValue("Regels code kortste oplossing", out Dictionary<int,int> dataFirstStatistic);
            // There are 2 candidates with 8 lines.
            Assert.AreEqual(2, dataFirstStatistic.GetValueOrDefault(8));
            // There is 1 candidate with 10 lines.
            Assert.AreEqual(1, dataFirstStatistic.GetValueOrDefault(10));
        }

        private async Task InsertAttempt(LevelSessionController levelSessionController, StatementController statementController, string id, int levelNumber, Statement[] statements)
        {
            await levelSessionController.StartLevel(id,levelNumber.ToString());
            await statementController.SaveAttempt(id, levelNumber, statements);
            await levelSessionController.StopLevel(id, levelNumber.ToString());
        }
    }
}
