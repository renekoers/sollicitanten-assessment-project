using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTest
{
    [TestClass]
    public class LevelTest
    {
        [TestMethod]
        public void LevelNumberTest()
        {
            Level level = Level.Get(1);
            Assert.AreEqual(1, level.LevelNumber);
        }


    }
}
