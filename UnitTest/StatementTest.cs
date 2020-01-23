using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    class StatementTest
    {
        [TestMethod]
        public void GetLevelTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
        }
    }
}
