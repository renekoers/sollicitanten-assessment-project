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
    }
}
