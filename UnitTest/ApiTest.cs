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
        public void AddCandidateTest()
        {
            Api.AddCandidate("Test kandidaat");
            Candidate candidate = Api.GetCandidate();
            Api.StartSession(candidate.ID);
            Assert.AreEqual("Test kandidaat", candidate.Name);
        }
        [TestMethod]
        public void GetCandidateWithoutAddingTest()
        {
            Assert.IsNull(Api.GetCandidate());
        }
        [TestMethod]
        public void StartSessionNoCandidateTest()
        {
            Assert.IsFalse(Api.StartSession(0));
        }
        [TestMethod]
        public void StartSessionWithCandidateTest()
        {
            Api.AddCandidate("Test kandidaat 2");
            Assert.IsTrue(Api.StartSession(Api.GetCandidate().ID));
        }
    }
}
