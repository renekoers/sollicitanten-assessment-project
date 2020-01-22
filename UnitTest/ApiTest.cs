using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System;
using Newtonsoft.Json.Linq;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void GetLevelTest()
        {
            Assert.AreEqual(2, 1 * 2);
        }

        [TestMethod]
        public void RunSingleCommandTest()
        {
            dynamic result = JObject.Parse(Api.RunCommands(1, new string[] { "RotateLeft" }));
            Assert.AreEqual(result.States.Count, 1);
        }
        [TestMethod]
        public void RunMultipleCommandsTest()
        {
            dynamic result = JObject.Parse(Api.RunCommands(1, new string[]{"RotateLeft", "RotateRight"}));
            Assert.AreEqual(result.States.Count, 2);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RunNonExistingCommandTest()
        {
            Api.RunCommands(1, new string[] { "NoCommand" });
        }
    }
}
