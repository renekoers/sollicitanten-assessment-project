using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class LevelSolutionTest
    {
        [TestMethod]
        public void RunSingleCommandTest()
        {
            LevelSolution solution = new LevelSolution(1, new Statement[] { new SingleCommand(Command.RotateLeft) });
            List<IState> states = solution.States;
            Assert.AreEqual(2, states.Count);
        }
        [TestMethod]
        public void RunMultipleCommandsTest()
        {
            LevelSolution solution = new LevelSolution(1, new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.MoveForward), new SingleCommand(Command.RotateRight) });
            List<IState> states = solution.States;
            Assert.AreEqual(4, states.Count);
        }
    }
}
