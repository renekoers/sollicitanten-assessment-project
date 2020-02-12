using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void GetLevelNumberTest()
        {
            int ID = Api.StartSession();
            IState level = Api.StartLevelSession(ID, 1);
            Assert.AreEqual(1, level.PuzzleLevel);
        }
        [TestMethod]
        public void EndSessionTest()
        {
            Api.AddCandidate("Test kandi");
            int ID = Api.GetCandidate().ID;
            Api.StartSession(ID);
            Api.EndSession(ID);
            GameSession gameSession = Api.GetSession(ID);
            Assert.IsFalse(gameSession.InProgress);
        }

        [TestMethod]
        public void AddCandidateTest()
        {
            Api.AddCandidate("Test kandidaat");
            Candidate candidate = Api.GetCandidate();
            Api.StartSession(candidate.ID);
            Assert.AreEqual("Test kandidaat", candidate.Name);
        }
        [TestMethod]
        public void GetCandidateWithoutAnyCandidateIsNullTest()
        {
            Assert.IsNull(Api.GetCandidate());
        }
        [TestMethod]
        public void StartSessionWithInvalidIDIsFalseTest()
        {
            Assert.IsFalse(Api.StartSession(-1));
        }
        [TestMethod]
        public void StartSessionWithCandidateTest()
        {
            Api.AddCandidate("Test kandidaat 2");
            Assert.IsTrue(Api.StartSession(Api.GetCandidate().ID));
        }
        [TestMethod]
        public void GetCandidateWithoutFreeSessionIsNullTest()
        {
            Api.AddCandidate("Test kandidaat 3");
            Api.StartSession(Api.GetCandidate().ID);
            Assert.IsNull(Api.GetCandidate());
        }
        [TestMethod]
        public void GetLastIDWhichIsFinishedTest()
        {
            Api.AddCandidate("Test kandidaat First");
            Api.AddCandidate("Test kandidaat Last");
            int firstID = Api.GetCandidate().ID;
            Api.StartSession(firstID);
            int lastID = Api.GetCandidate().ID;
            Api.StartSession(lastID);
            Api.EndSession(firstID);
            Api.EndSession(lastID);
            Assert.AreEqual(lastID,Api.GetLastIDWhichIsFinished().Value);
        }
        [TestMethod]
        public void GetPreviousIDBeforeFirstCandidateIsNullTest()
        {
            Api.AddCandidate("Test kandidaat");
            int id = Api.GetCandidate().ID;
            Api.StartSession(id);
            Api.EndSession(id);
            Assert.IsNull(Api.GetPreviousIDWhichIsFinished(1));
        }
        [TestMethod]
        public void GetPreviousIDWhichIsFinishedTest()
        {
            long time = Api.GetEpochTime();
            Api.AddCandidate("Test kandidaat 1");
            int id1 = Api.GetCandidate().ID;
            Api.StartSession(id1);
            Api.AddCandidate("Test kandidaat 2");
            int id2 = Api.GetCandidate().ID;
            Api.StartSession(id2);
            Api.AddCandidate("Test kandidaat 3");
            int id3 = Api.GetCandidate().ID;
            Api.StartSession(id3);
            Api.AddCandidate("Test kandidaat 4");
            int id4 = Api.GetCandidate().ID;
            Api.StartSession(id4);
            Api.AddCandidate("Test kandidaat 5");
            int id5 = Api.GetCandidate().ID;
            Api.StartSession(id5);
            Api.EndSession(id1);
            Api.EndSession(id2);
            Api.EndSession(id4);
            Api.EndSession(id5);
            Assert.AreEqual(id2,Api.GetPreviousIDWhichIsFinished(id4));
        }
        [TestMethod]
        public void GetNextIDAfterLastCandidateIsNullTest()
        {
            Api.AddCandidate("Test kandidaat");
            int id = Api.GetCandidate().ID;
            Api.StartSession(id);
            Api.EndSession(id);
            Assert.IsNull(Api.GetNextIDWhichIsFinished(id));
        }
        [TestMethod]
        public void GetNextIDWhichIsFinishedTest()
        {
            Api.AddCandidate("Test kandidaat 1");
            int id1 = Api.GetCandidate().ID;
            Api.StartSession(id1);
            Api.AddCandidate("Test kandidaat 2");
            int id2 = Api.GetCandidate().ID;
            Api.StartSession(id2);
            Api.AddCandidate("Test kandidaat 3");
            int id3 = Api.GetCandidate().ID;
            Api.StartSession(id3);
            Api.AddCandidate("Test kandidaat 4");
            int id4 = Api.GetCandidate().ID;
            Api.StartSession(id4);
            Api.AddCandidate("Test kandidaat 5");
            int id5 = Api.GetCandidate().ID;
            Api.StartSession(id5);
            Api.EndSession(id1);
            Api.EndSession(id2);
            Api.EndSession(id4);
            Api.EndSession(id5);
            Assert.AreEqual(id4,Api.GetNextIDWhichIsFinished(id2).Value);
        }
    }
}
