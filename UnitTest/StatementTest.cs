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
            List<IState> states = (new LevelSolution(1, statements)).States;
            IState finalState = states[states.Count - 1];
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
            List<IState> states = (new LevelSolution(1, statements)).States;
            IState finalState = states[states.Count - 1];
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
            List<IState> states = (new LevelSolution(1, statements)).States;
            IState finalState = states[states.Count - 1];
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
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(1, states.Count);
        }
        [TestMethod]
        public void WhileWithFalseConditionTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Statement[] statements = new Statement[] { new While(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(1, states.Count);
        }
        [TestMethod]
        public void DoWhileWithFalseConditionTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileLeft, ConditionValue.Passable);
            Statement[] statements = new Statement[] { new DoWhile(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(2, states.Count);
        }
        [TestMethod]
        public void RepeatThriceTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            Direction initialDir = puzzle.Character.Direction;
            Statement[] statements = new Statement[] { new Repeat(3, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(4, states.Count);
            Assert.AreEqual(initialDir.Right(), states[3].Character.DirectionCharacter);
        }
        [TestMethod]
        public void StatesEqualsTest()
        {
            Statement[] statements = new Statement[]{new SingleCommand(Command.RotateLeft)};
            List<IState> states1 = (new LevelSolution(1, statements)).States;
            List<IState> states2 = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(states1[1],states2[1]);
        }
        [TestMethod]
        public void InfiniteLoopTest()
        {
            Statement[] statements = new Statement[] { new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft),new SingleCommand(Command.RotateRight) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.IsTrue(states.Count<6);
        }
        [TestMethod]
        public void InfiniteLoopMultipleIterationsTest()
        {
            Statement[] statements = new Statement[] { new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.IsTrue(states.Count<7);
        }
        [TestMethod]
        public void InfiniteLoopInNestedLoopTest()
        {
            Statement[] statements = new Statement[] {new Repeat(10, new Statement[] {new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft) })}) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.IsTrue(states.Count<7);
        }
        [TestMethod]
        public void MaxAmountStatesTest()
        {
            Statement[] statements = new Statement[]{new Repeat(Statement.MaxStates+2, new Statement[]{new SingleCommand(Command.MoveForward)})};
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual((int)Statement.MaxStates + 1, states.Count);
        }
    }
}
