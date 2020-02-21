using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BackEnd
{
	public partial class Database
	{
		/// Retrieving IDs
		async internal static Task<List<string>> GetFinishedIDsAfterTime(DateTime time)
		{
			IEnumerable<CandidateEntity> newFinishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Match(candidate => (candidate.finished > time))
					.Sort(candidate => candidate.finished, Order.Ascending)
					.ExecuteAsync();
			return newFinishedCandidates.Select(candidate => candidate.ID).ToList();
		}
		async internal static Task<string> GetPreviousFinishedID(string ID)
		{
			DateTime defaultTime = new DateTime();
			CandidateEntity currentCandidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			if (currentCandidate.finished == defaultTime)
			{
				return null;
			}
			IEnumerable<CandidateEntity> previousFinishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Match(candidate => (candidate.finished < currentCandidate.finished) && (candidate.finished > defaultTime))
					.Sort(candidate => candidate.finished, Order.Descending)
					.ExecuteAsync();
			return previousFinishedCandidates.Count() > 0 ? previousFinishedCandidates.First().ID : null;
		}
		async internal static Task<string> GetNextFinishedID(string ID)
		{
			CandidateEntity currentCandidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			if (currentCandidate.finished == new DateTime())
			{
				return null;
			}
			IEnumerable<CandidateEntity> nextFinishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Match(candidate => (candidate.finished > currentCandidate.finished))
					.Sort(candidate => candidate.finished, Order.Ascending)
					.ExecuteAsync();
			return nextFinishedCandidates.Count() > 0 ? nextFinishedCandidates.First().ID : null;
		}
		async internal static Task<string> GetLastFinishedID()
		{
			IEnumerable<CandidateEntity> finishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Sort(candidate => candidate.finished, Order.Descending)
					.ExecuteAsync();
			return finishedCandidates.Count() > 0 ? finishedCandidates.First().ID : null;
		}

		/// Making statistics

		
        /// <summary>
        /// Creates a list of functions that maps a levelSession to an int. This function are used in constructing dictionaries that represents the statistics.
        /// Add here extra functions in order to add extra statistics.
        /// </summary>
        /// <returns>Dictionary with for name of the statistic creates data that represents the statistic of the given level.</returns>
        private static Dictionary<string,Func<LevelSession,int>> GetStatisticsFunctions()
        {
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = new Dictionary<string,Func<LevelSession, int>>();
            statisticsFunctions.Add("Regels code kortste oplossing", LevelSession.GetLines);
            statisticsFunctions.Add("Tijd tot korste oplossing", LevelSession.GetDuration);
            statisticsFunctions.Add("Pogingen tot korste oplossing", session => session.NumberOfAttemptsForFirstSolved);
            return statisticsFunctions;
        }
	}
}