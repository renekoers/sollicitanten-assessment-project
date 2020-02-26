using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class AnalysisTest
    {
        // [TestMethod]
        // public void SingleCommandLineCountTest()
        // {
        //     List<Statement> statements = new List<Statement>() { new SingleCommand(Command.RotateLeft) };
        //     Analysis analysis = new Analysis(statements);
        //     Assert.AreEqual(1, analysis.LinesOfCode);
        // }

        // [TestMethod]
        // public void SeveralCommandLineCountTest()
        // {
        //     List<Statement> statements = new List<Statement>
        //     {
        //         new Repeat(2, new Statement[] { new SingleCommand(Command.RotateRight) }),
        //         new SingleCommand(Command.RotateLeft)
        //     };
        //     Analysis analysis = new Analysis(statements);
        //     Assert.AreEqual(3, analysis.LinesOfCode);
        // }
    }
}
