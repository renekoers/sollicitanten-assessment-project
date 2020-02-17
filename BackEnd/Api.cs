using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		async public static Task<bool> AddCandidate(string name)
		{
			return await Repository.AddCandidate(name);
		}
		async public static Task<CandidateEntity> GetCandidate()
		{
			return await Repository.GetCandidate();
		}
		async public static Task<CandidateEntity> GetCandidate(string ID)
		{
			return await Repository.GetCandidate(ID);
		}
		async public static Task<bool> IsUnstarted(string ID)
		{
			return await Repository.IsUnstarted(ID);
		}
		async public static Task<List<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return await Database.GetAllUnstartedCandidate();
		}

		/// <summary>
		/// Start a new session for a new CandidateEntity.!-- This method is used in mockdata and tests!!!!
		/// </summary>
		/// <returns>The ID of the newly created CandidateEntity used to access their session</returns>
		[Obsolete("Use StartSession(string ID) instead")]
		public static string StartSession()
		{
			return Repository.CreateSession();
		}
		async public static Task<bool> StartSession(string ID)
		{
			return await Repository.StartSession(ID);
		}

		/// <summary>
		/// Obtain the session from a CandidateEntity. This session should at some point be stopped, and can be used to check the duration etc.
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static GameSession GetSession(string ID)
		{
			return Repository.GetSession(ID);
		}
		public static bool IsStarted(string ID)
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
		public static IState StartLevelSession(string ID, int levelNumber)
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
		public static LevelSolution SubmitSolution(string ID, int levelNumber, Statement[] statements)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			LevelSolution solution = new LevelSolution(levelNumber, statements);
			levelSession.Attempt(solution);
			Repository.UpdateSession(ID, gameSession);
			return solution;
		}

		public static void PauseLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.Pause();
			Repository.UpdateSession(ID, gameSession);
		}
		public static IState ContinueLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.Restart();
			Repository.UpdateSession(ID, gameSession);
			return new State(new Puzzle(Level.Get(levelNumber)));
		}

		public static void EndLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.End();
			Repository.UpdateSession(ID, gameSession);
		}

		async public static Task<bool> EndSession(string ID)
		{
			if(await Database.EndSession(ID))
			{
				GameSession gameSession = GetSession(ID);
				gameSession.End();
				Repository.UpdateSession(ID, gameSession);
				return true;
			} else {
				return false;
			}
		}
		public static bool LevelHasBeenStarted(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			return gameSession.GetSession(levelNumber) != null;
		}
		public static bool LevelIsSolved(string ID, int levelNumber)
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
		async public static Task<List<string>> GetFinishedIDsAfterTime(DateTime time)
		{
			return await Database.GetFinishedIDsAfterTime(time);
		}
		/// <summary>
		/// This method finds the first ID of the CandidateEntity that ended the session after the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		public static int? GetNextIDWhichIsFinished(int ID)
		{
			return Repository.GetNextIDWhichIsFinished(ID);
		}
		/// <summary>
		/// This method finds the last ID of the CandidateEntity that ended the session before the given ID.
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
		public static Overview GetOverview(string ID)
		{
			return new Overview(GetSession(ID));
		}

		/// <summary>
		/// Gets the number of lines that a CandidateEntity needed to solve a specific level
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="levelNumber"></param>
		/// <returns></returns>
		public static int NumberOfLinesSolved(string ID, int levelNumber)
		{
			return GetSession(ID).GetSession(levelNumber).GetLeastLinesOfCodeSolution().Lines;
		}

		public static Dictionary<int, int> NumberOfLinesSolvedLevelsOf(string ID)
		{
			Dictionary<int, int> amountOfLinesByLevel = new Dictionary<int, int>();
			GameSession gameSession = GetSession(ID);
			foreach (int levelNumber in gameSession.SolvedLevelNumbers)
			{
				amountOfLinesByLevel.Add(levelNumber, NumberOfLinesSolved(ID, levelNumber));
			}
			return amountOfLinesByLevel;
		}

		/// <summary>
		/// Creates a dictionary labeled by level number with as entries the tallies for the shortest code solutions
		/// </summary>
		/// <param name="levelNumbers"></param>
		/// <returns></returns>
		public static Dictionary<int, Dictionary<int, int>> TallyEveryoneNumberOfLines(ISet<int> levelNumbers)
		{
			Dictionary<int, Dictionary<int, int>> talliesByLevel = new Dictionary<int, Dictionary<int, int>>();
			foreach (int levelNumber in levelNumbers)
			{
				talliesByLevel.Add(levelNumber, Repository.TallyEveryoneNumberOfLines(levelNumber));
			}
			return talliesByLevel;
		}


		/// <summary>
		/// Creates a dictionary containing only the solved levels of ID with tallies for the shortest code solution
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static Dictionary<int, Dictionary<int, int>> TallyEveryoneNumberOfLinesSolvedLevelsOf(string ID)
		{
			GameSession gameSession = GetSession(ID);
			ISet<int> solvedLevels = gameSession.SolvedLevelNumbers;
			return TallyEveryoneNumberOfLines(solvedLevels);
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
		public static IEnumerable<Statement> ParseStatementTreeJson(System.Text.Json.JsonElement statementTreeJson)
	   => StatementParser.ParseStatementTreeJson(statementTreeJson);
	}
}
