using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		async public static Task<bool> AddCandidate(string name)
		{
			return await Database.AddNewCandidate(name) != null;
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
		/// This method finds the last ID of the CandidateEntity that ended the session before the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		async public static Task<string> GetPreviousFinishedID(string ID)
		{
			return await Database.GetPreviousFinishedID(ID);
		}
		/// <summary>
		/// This method finds the first ID of the CandidateEntity that ended the session after the given ID.
		/// </summary>
		/// <returns>ID if there exists one</returns>
		async public static Task<string> GetNextFinishedID(string ID)
		{
			return await Database.GetNextFinishedID(ID);
		}
		async public static Task<string> GetLastFinishedID()
		{
			return await Database.GetLastFinishedID();
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
    }
}