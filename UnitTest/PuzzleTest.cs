using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class PuzzleTest
    {
        [TestMethod]
        public void LevelNumberTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            Assert.AreEqual(1, puzzle.LevelNumber);
        }
        [TestMethod]
        public void Test()
        {
            Puzzle p = new Puzzle(Level.Get(1));
            Assert.AreNotEqual("", Level.Get(1));
        }

        
    }
}
