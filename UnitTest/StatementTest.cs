using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class StatementTest
    {
        [TestMethod]
        public void IfTrueElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testTrue = character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Direction currentDirection = character.Direction;
            Statement[] statements = new Statement[] { new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testTrue, new Statement[] { new SingleCommand(Command.RotateLeft) }, new Statement[] { new SingleCommand(Command.RotateRight) }) };
            List<State> states = Api.RunCommands(1, statements);
            State finalState = states[states.Count - 1];
            Direction newDirection = finalState.Character.DirectionCharacter;
            Assert.AreEqual(currentDirection.Left(), newDirection);
        }
        [TestMethod]
        public void IfTrueNoElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testTrue = character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Direction currentDirection = character.Direction;
            Statement[] statements = new Statement[] { new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testTrue, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<State> states = Api.RunCommands(1, statements);
            State finalState = states[states.Count - 1];
            Direction newDirection = finalState.Character.DirectionCharacter;
            Assert.AreEqual(currentDirection.Left(), newDirection);
        }
        [TestMethod]
        public void IfFalseElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Direction currentDirection = character.Direction;
            Statement[] statements = new Statement[] { new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }, new Statement[] { new SingleCommand(Command.RotateRight) }) };
            List<State> states = Api.RunCommands(1, statements);
            State finalState = states[states.Count - 1];
            Direction newDirection = finalState.Character.DirectionCharacter;
            Assert.AreEqual(currentDirection.Right(), newDirection);
        }
        [TestMethod]
        public void IfFalseNoElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Statement[] statements = new Statement[] { new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<State> states = Api.RunCommands(1, statements);
            Assert.AreEqual(0, states.Count);
        }
        [TestMethod]
        public void WhileWithFalseConditionTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Statement[] statements = new Statement[] { new While(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<State> states = Api.RunCommands(1, statements);
            Assert.AreEqual(0, states.Count);
        }
        [TestMethod]
        public void DoWhileWithFalseConditionTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileLeft, ConditionValue.Passable);
            Statement[] statements = new Statement[] { new DoWhile(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<State> states = Api.RunCommands(1, statements);
            Assert.AreEqual(1, states.Count);
        }
        [TestMethod]
        public void RepeatThriceTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            Statement[] statements = new Statement[] { new Repeat(3, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<State> states = Api.RunCommands(1, statements);
            Assert.AreEqual(3, states.Count);
        }
    }
}
