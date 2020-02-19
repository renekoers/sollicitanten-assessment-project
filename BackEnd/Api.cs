using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
			return await Database.AddNewCandidate(name) != null;
		}
		async public static Task<CandidateEntity> GetCandidate()
		{
			return await Database.GetCandidate();
		}
		async public static Task<CandidateEntity> GetCandidate(string ID)
		{
			return await Database.GetCandidate(ID);
		}
		async public static Task<bool> IsUnstarted(string ID)
		{
			return await Database.IsUnstarted(ID);
		}
		async public static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return await Database.GetAllUnstartedCandidate();
		}
		public static IState GetTutorialLevel()
		{
			return new State(new Puzzle(Level.Get(0)));
		}
		public static LevelSolution SubmitSolutionTutorial(Statement[] statements)
		{
			return new LevelSolution(0, new StatementBlock(statements), 0);
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
			return await Database.StartSession(ID);
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
		/// Returns true if candidate has yet to start a session.
		///</summary>
		///<param name="ID"></param>
		///<returns>Bool</returns>
		async public static Task<bool> HasCandidateNotYetStarted(string ID)
		{
			return await Database.HasCandidateNotYetStarted(ID);
		}
		///<summary>
		/// Returns true if candidate still has an active session available.
		///</summary>
		///<param name="ID"></param>
		///<returns>Bool</returns>
		async public static Task<bool> IsCandidateStillActive(string ID)
		{
			return await Database.IsCandidateStillActive(ID);
		}

		/// <summary>
		/// Starts a level session.
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="levelNumber"></param>
		/// <returns></returns>
		async public static Task<IState> StartLevelSession(string ID, int levelNumber)
		{
			return (await Session.StartLevel(ID, levelNumber)) ? new State(new Puzzle(Level.Get(levelNumber))) : null;
		}
		/// <summary>
		/// Stops a level session.
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="levelNumber"></param>
		/// <returns></returns>
		async public static Task<bool> StopLevelSession(string ID, int levelNumber)
		{
			return await Session.StopLevel(ID, levelNumber);
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
			LevelSolution solution = levelSession.Attempt(statements);
			Repository.UpdateSession(ID, gameSession);
			return solution;
		}
		[Obsolete("Remove function after mock data is fixed!!")]
		public static void EndLevelSession(string ID, int levelNumber)
		{
			GameSession gameSession = GetSession(ID);
			LevelSession levelSession = gameSession.GetSession(levelNumber);
			levelSession.End();
			Repository.UpdateSession(ID, gameSession);
		}

		async public static Task<bool> EndSession(string ID)
		{
			return await Database.EndSession(ID);
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
			return await Statistics.GetFinishedIDsAfterTime(time);
		}
		/// <summary>
		/// This method finds the first ID of the CandidateEntity that ended the session after the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		async public static Task<string> GetNextFinishedID(string ID)
		{
			return await Statistics.GetNextFinishedID(ID);
		}
		/// <summary>
		/// This method finds the last ID of the CandidateEntity that ended the session before the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		async public static Task<string> GetPreviousFinishedID(string ID)
		{
			return await Statistics.GetPreviousFinishedID(ID);
		}
		async public static Task<string> GetLastFinishedID()
		{
			return await Statistics.GetLastFinishedID();
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
		/// Creates a dictionary consisting of all statistics of a candidate.
		/// </summary>
		/// <returns>Dictionary with for every level has a dictionary of name of the statistic and the data.</returns>
		public static Dictionary<int, Dictionary<string, int>> StatisticsCandidate(string ID)
		{
			return Analysis.MakeStatisticsCandidate(ID);
		}
		/// <summary>
		/// Creates a dictionary consisting of all statistics of all candidates.
		/// </summary>
		/// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
		public static Dictionary<int, Dictionary<string, Dictionary<int, int>>> StatisticsEveryone()
		{
			return Analysis.MakeStatisticsEveryone();
		}
		/// <summary>
		/// This method creates a dictionary consisting of the number of candidates that did not solve a certain level.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, int> NumberOfCandidatesNotSolvedPerLevel()
		{
			return Repository.NumberOfCandidatesNotSolvedPerLevel();
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
		public static IEnumerable<Statement> ParseStatementTreeJson(System.Text.Json.JsonElement statementTreeJson)
	   => StatementParser.ParseStatementTreeJson(statementTreeJson);
	}
}
