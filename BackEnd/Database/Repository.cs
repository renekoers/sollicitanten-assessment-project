using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

		[Obsolete("Use StartSession(int ID) instead!")]
		public static string CreateSession()
		{
			string ID = "1";
			GameSessions.Add("1", new GameSession());
			return ID;
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
	}
}
