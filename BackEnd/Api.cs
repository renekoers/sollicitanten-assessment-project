using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace BackEnd
{
	public class Api
	{
		public static bool ValidateUser(string username, string hashedPass)
		{
			return Repository.ValidateUser(username, hashedPass);
		}
		public static void AddCandidate(string name)
		{
			Repository.AddCandidate(name);
		}
		public static Candidate GetCandidate()
		{
			return Repository.GetCandidate();
		}
		public static Candidate GetCandidate(int ID)
		{
			return Repository.GetCandidate(ID);
		}
		public static bool IsUnstarted(int ID)
		{
			return Repository.IsUnstarted(ID);
		}
		/// <summary>
		/// Start a new session for a new candidate.!-- This method is used in mockdata and tests!!!!
		/// </summary>
		/// <returns>The ID of the newly created candidate used to access their session</returns>
		[Obsolete("Use StartSession(int ID) instead")]
		public static int StartSession()
		{
			return Repository.CreateSession();
		}
		public static bool StartSession(int ID)
		{
			return Repository.StartSession(ID);
		}

		/// <summary>
		/// Obtain the session from a candidate. This session should at some point be stopped, and can be used to check the duration etc.
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static GameSession GetSession(int ID)
		{
			return Repository.GetSession(ID);
		}
		public static bool IsStarted(int ID)
		{
			GameSession gameSession = Api.GetSession(ID);
			if (gameSession == null)
			{
				return false;
			}
			return gameSession.InProgress;
		}
		///<summary>
		/// Makes a tutorial session.
		/// </summary>
		public static void StartTutorialSession()
		{
			Repository.CreateTutorialSession();
		}

		/// <summary>
		/// Begin a new level session.
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="levelNumber"></param>
		/// <returns></returns>
		public static IState StartLevelSession(int ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = new LevelSession(levelNumber);
			gameSession.AddLevel(levelSession);
			Repository.UpdateSession(ID, gameSession);

			return new State(new Puzzle(Level.Get(levelNumber)));
		}

        /// <summary>
        /// Submit a new solution attempt
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="levelNumber"></param>
        /// <param name="statements"></param>
        /// <returns>A LevelSolution object which contains amongst other things a list of IState objects and whether the level was solved or not</returns>
        public static LevelSolution SubmitSolution(int ID, int levelNumber, Statement[] statements)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = gameSession.GetSession(levelNumber);
            LevelSolution solution = levelSession.Attempt(statements);
            Repository.UpdateSession(ID, gameSession);
            return solution;
        }

		public static void PauseLevelSession(int ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.Pause();
			Repository.UpdateSession(ID, gameSession);
		}
		public static IState ContinueLevelSession(int ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.Restart();
			Repository.UpdateSession(ID, gameSession);
			return new State(new Puzzle(Level.Get(levelNumber)));
		}

		public static void EndLevelSession(int ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.End();
			Repository.UpdateSession(ID, gameSession);
		}

		public static void EndSession(int ID)
		{
			GameSession gameSession = GetSession(ID);
			gameSession.End();
			Repository.UpdateSession(ID, gameSession);
		}
		public static bool LevelHasBeenStarted(int ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			return gameSession.GetSession(levelNumber) != null;
		}
		public static bool LevelIsSolved(int ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			if (levelSession == null)
			{
				return false;
			}
			return levelSession.Solved;
		}

		/// <summary>
		/// This method creates a list of all IDs of candidates that finished a session after a given time
		/// </summary>
		/// <returns>List of IDs</returns>
		public static List<int> GetFinishedIDsAfterEpochTime(long epochTime)
		{
			return Repository.GetFinishedIDsAfterEpochTime(epochTime);
		}
		/// <summary>
		/// This method finds the first ID of the candidate that ended the session after the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		public static int? GetNextIDWhichIsFinished(int ID)
		{
			return Repository.GetNextIDWhichIsFinished(ID);
		}
		/// <summary>
		/// This method finds the last ID of the candidate that ended the session before the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		public static int? GetPreviousIDWhichIsFinished(int ID)
		{
			return Repository.GetPreviousIDWhichIsFinished(ID);
		}
		public static int? GetLastIDWhichIsFinished()
		{
			return Repository.GetLastIDWhichIsFinished();
		}
		public static int GetTotalLevelAmount()
		{
			int totalLevels = Level.TotalLevels;
			return totalLevels;
		}
		public static Overview GetOverview(int ID)
		{
			return new Overview(GetSession(ID));
		}
        /// <summary>
        /// Creates a dictionary consisting of all statistics of a candidate.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the data.</returns>
        public static Dictionary<int,Dictionary<string,int>> StatisticsCandidate(int ID)
        {
            return Analysis.StatisticsCandidate(ID);
        }
        /// <summary>
        /// Creates a dictionary consisting of all statistics of all candidates.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
        public static Dictionary<int,Dictionary<string, Dictionary<int, int>>> StatisticsEveryone(int ID)
        {
            return Analysis.StatisticsEveryone(ID);
        }

		public static long GetEpochTime()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		}

		/// <summary>
		/// Converts a string into an array of commands.
		/// </summary>
		/// <param name="input">String of commands seperated by ';'.</param>
		/// <returns>Array of commands.</returns>
		/// <exception cref="ArgumentException">Throws exception if the string is not a command.</exception>
		private static SingleCommand[] ConvertStringToSingleCommands(string[] input)
		{
			SingleCommand[] commands = new SingleCommand[input.Length];
			for (int index = 0; index < input.Length; index++)
			{
				commands[index] = new SingleCommand((Command)Enum.Parse(typeof(Command), input[index].Trim()));
			}
			return commands;
		}

		private static int CalculateScore(Statement[] input)
		{
			int lines = 0;
			foreach (Statement statement in input)
			{
				lines += statement.GetLines();
			}
			return lines;
		}

		/// <summary>
		/// Used to insert some solutions of the levels to see the graphs
		/// </summary>
		private static void InsertMockData()
		{
			int id = StartSession();
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
			int idOther = StartSession();
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
		public static IEnumerable<Statement> ParseStatementTreeJson(System.Text.Json.JsonElement statementTreeJson)
	   => StatementParser.ParseStatementTreeJson(statementTreeJson);
	}
}
