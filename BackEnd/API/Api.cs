using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		public static bool ValidateUser(string username, string hashedPass)
		{
			return Repository.ValidateUser(username, hashedPass);
		}
		public static IState GetTutorialLevel()
		{
			return new State(new Puzzle(Level.Get(0)));
		}
		public static LevelSolution SubmitSolutionTutorial(Statement[] statements)
		{
			return new LevelSolution(0, new StatementBlock(statements), 0);
		}



		public static long GetEpochTime()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		}

		/// <summary>
		/// Used to insert some solutions of the levels to see the graphs
		/// </summary>
		private static void InsertMockData()
		{
			string id = StartSession();
			StartLevelSession(id, 1);
			SubmitSolution(id, 1, new Statement[]
			{
				new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
				{
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.PickUp),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.Drop),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.MoveForward)
				})
			});
			EndLevelSession(id, 1);
			StartLevelSession(id, 2);
			SubmitSolution(id, 2, new Statement[]
			{
				new While(ConditionParameter.TileLeft, ConditionValue.Impassable, true, new Statement[]
				{
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.PickUp),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.Drop),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.MoveForward)
				}),
				new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
				{
					new While(ConditionParameter.TileFront, ConditionValue.Passable, true, new Statement[]
					{
						new SingleCommand(Command.MoveForward)
					}),
					new SingleCommand(Command.RotateRight)
				})
			});
			EndLevelSession(id, 2);
			EndSession(id);
			string idOther = StartSession();
			StartLevelSession(idOther, 1);
			SubmitSolution(idOther, 1, new Statement[]
			{
				new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
				{
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.PickUp),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.Drop),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.MoveForward),
					new SingleCommand(Command.MoveBackward),
					new SingleCommand(Command.MoveForward)
				})
			});
			EndLevelSession(idOther, 1);
			EndSession(idOther);
		}
	}
}
