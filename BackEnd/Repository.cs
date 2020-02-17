﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson;

namespace BackEnd
{
	/// <summary>
	/// Currently this is a mock for keeping data in memory without any proper persistence, but this should talk to the database, once we have one
	/// </summary>
	public class Repository
	{
		private Repository() { }
		private static readonly Dictionary<string, GameSession> GameSessions = new Dictionary<string, GameSession>();

		/// <summary>
		/// This method should validate the credentials of HR. This will later be implemented!!!!!!
		/// </summary>
		/// <returns></returns>
		internal static bool ValidateUser(string username, string hashedPassword)
		{
			return true;
		}		
		/// <summary>
		/// This method finds the first ID of the candidate that ended the session after the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		internal static int? GetNextIDWhichIsFinished(int ID)
		{
			return null;
			// Util.Min(GameSessions.Where(pair => !pair.Value.InProgress && pair.Key > ID).Select(pair => pair.Key).ToList());
		}
		/// <summary>
		/// This method finds the last ID of the candidate that ended the session before the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		internal static int? GetPreviousIDWhichIsFinished(int ID)
		{
			return null;
			// Util.Max(GameSessions.Where(pair => !pair.Value.InProgress && pair.Key < ID).Select(pair => pair.Key).ToList());
		}
		internal static int? GetLastIDWhichIsFinished()
		{
			return null;
			// Util.Max(GameSessions.Where(pair => !pair.Value.InProgress).Select(pair => pair.Key).ToList());
		}

		[Obsolete("Use StartSession(int ID) instead!")]
		public static string CreateSession()
		{
			string ID = "1";
			GameSessions.Add("1", new GameSession());
			return ID;
		}
		async public static Task<bool> StartSession(string ID)
		{
			if (await Database.StartSession(ID))
			{
				GameSessions.Add(ID, new GameSession());
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void CreateTutorialSession()
		{
			if (!GameSessions.ContainsKey("0"))
			{
				GameSessions.Add("0", new GameSession());
			}
			else
			{
				GameSessions["0"] = new GameSession();
			}

		}

		public static GameSession GetSession(string ID)
		{
			return GameSessions.TryGetValue(ID, out GameSession session) ? session : null;
		}

		public static bool UpdateSession(string ID, GameSession session)
		{
			if (GameSessions.ContainsKey(ID))
			{
				GameSessions[ID] = session;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Tallies the number of lines of the best solution for the given level number over all sessions
		/// </summary>
		/// <param name="levelNumber"></param>
		/// <returns>A dictionary with as the first int the number of lines and the second int the number of people that solved the level in said amount of lines</returns>
		public static Dictionary<int, int> TallyEveryoneNumberOfLines(int levelNumber)
		{
			Dictionary<int, int> tally = new Dictionary<int, int>();
			foreach (GameSession gameSession in GameSessions.Values)
			{
				LevelSession levelSession = gameSession.GetSession(levelNumber);
				if (levelSession is null)
				{
					continue;
				}
				LevelSolution leastLinesSolution = levelSession.GetLeastLinesOfCodeSolution();
				if (leastLinesSolution is null)
				{
					continue;
				}
				int lines = leastLinesSolution.Lines;
				if (tally.ContainsKey(lines))
				{
					tally[lines]++;
				}
				else
				{
					tally[lines] = 1;
				}
			}
			return tally;
		}
	}
}
