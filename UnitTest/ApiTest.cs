﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void GetLevelNumberTest()
        {
            IState level = Api.GetLevel(1);
            Assert.AreEqual(1, level.PuzzleLevel);
        }
        [TestMethod]
        public void RunSingleCommandTest()
        {
            List<IState> states = Api.RunCommands(1, new Statement[] { new SingleCommand(Command.RotateLeft) });
            Assert.AreEqual(1, states.Count);
        }
        [TestMethod]
        public void RunMultipleCommandsTest()
        {
            List<IState> states = Api.RunCommands(1, new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.MoveForward), new SingleCommand(Command.RotateRight) });
            Assert.AreEqual(3, states.Count);
        }
    }
}
