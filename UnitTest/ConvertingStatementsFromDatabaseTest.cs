using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BackEnd;

namespace UnitTest
{
    [TestClass]
    public class ConvertingStatementsFromDatabaseTest
    {
        [TestMethod]
        public void CompletingPropertiesOfMoveForwardTest()
        {
            Level level = Level.CreateFromString("" +
				"!......\n" +
				".......\n" +
				"#...<..\n" +
                ".......\n" +
                ".......\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement statement = new SingleCommand(0); // SingleCommand with default values
            LevelSolution levelSolution = makeDefaultSolution(level, statement);
            statement.Command = Command.MoveForward.ToString();
            levelSolution.CompleteAllProperties();
            levelSolution.Execute(puzzle);
            Assert.AreEqual(2, levelSolution.States.Count);
            IState state = levelSolution.States[1];
            Assert.AreEqual(state.PuzzleTiles[17], state.Character.Tile); // The character starts at tile 18
        }

        [TestMethod]
        public void CompletingPropertiesOfRotateLeftTest()
        {
            Level level = Level.CreateFromString("" +
				"!......\n" +
				".......\n" +
				"#...<..\n" +
                ".......\n" +
                ".......\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement statement = new SingleCommand(0); // SingleCommand with default values
            LevelSolution levelSolution = makeDefaultSolution(level, statement);
            statement.Command = Command.RotateLeft.ToString();
            levelSolution.CompleteAllProperties();
            levelSolution.Execute(puzzle);
            Assert.AreEqual(2, levelSolution.States.Count);
            IState state = levelSolution.States[1];
            Assert.AreEqual(Direction.South, state.Character.DirectionCharacter);
        }

        [TestMethod]
        public void CompletingPropertiesOfRotateRightTest()
        {
            Level level = Level.CreateFromString("" +
				"!......\n" +
				".......\n" +
				"#...<..\n" +
                ".......\n" +
                ".......\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement statement = new SingleCommand(0); // SingleCommand with default values
            LevelSolution levelSolution = makeDefaultSolution(level, statement);
            statement.Command = Command.RotateRight.ToString();
            levelSolution.CompleteAllProperties();
            levelSolution.Execute(puzzle);
            Assert.AreEqual(2, levelSolution.States.Count);
            IState state = levelSolution.States[1];
            Assert.AreEqual(Direction.North, state.Character.DirectionCharacter);
        }

        [TestMethod]
        public void CompletingPropertiesOfIfElseSetsPropertiesWithConditionIsTrueTest()
        {
            Level level = Level.CreateFromString("" +
				"!......\n" +
				".......\n" +
				"#...<..\n" +
                ".......\n" +
                ".......\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement statement = new IfElse(0,0,false, new Statement[0]); // SingleCommand with default values
            LevelSolution levelSolution = makeDefaultSolution(level, statement);
            statement.Condition = new ConditionEntity(ConditionParameter.TileCurrent, ConditionValue.Passable, true);
            statement.Code = new Statement[]{new SingleCommand(Command.RotateLeft), new Else(new Statement[]{new SingleCommand(Command.RotateRight)})};
            levelSolution.CompleteAllProperties();
            levelSolution.Execute(puzzle);
            Assert.AreEqual(2, levelSolution.States.Count);
            IState state = levelSolution.States[1];
            Assert.AreEqual(Direction.South, state.Character.DirectionCharacter);
        }

		///<summary>
		/// Creates a levelsolution and sets the CodeBlock and States on null. LevelSolutions from the database satisfies this property
		///</summary>
        private LevelSolution makeDefaultSolution(Level level, Statement statement)
        {
            LevelSolution levelSolution = new LevelSolution(new Puzzle(level), new Statement[]{statement},0);
            levelSolution.CodeBlock = null;
            levelSolution.States = null;
            return levelSolution;
        }
    }
}
