using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void GetLevelPositionCharacterTest()
        {
            JObject level = JObject.Parse(Api.GetLevel(1));
            int[] positionCharacter = level["PositionCharacter"].Select(x => (int)x).ToArray();
            Assert.AreEqual(2,positionCharacter.Length);
        }
        [TestMethod]
        public void GetLevelLevelNumberTest()
        {
            JObject level = JObject.Parse(Api.GetLevel(1));
            var levelNumber = level["LevelNumber"];
            Assert.AreEqual(1,levelNumber);
        }

        [TestMethod]
        public void RunSingleCommandTest()
        {
            dynamic result = JObject.Parse(Api.RunCommands(1, new string[] { "RotateLeft" }));
            Assert.AreEqual(1,result.States.Count);
        }
        [TestMethod]
        public void RunMultipleCommandsTest()
        {
            dynamic result = JObject.Parse(Api.RunCommands(1, new string[]{"RotateLeft", "RotateRight"}));
            Assert.AreEqual(2,result.States.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RunNonExistingCommandTest()
        {
            Api.RunCommands(1, new string[] { "NoCommand" });
        }
    }
}
