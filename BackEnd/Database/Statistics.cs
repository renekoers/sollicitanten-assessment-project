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
			CandidateEntity currentCandidate = await GetCandidate(ID);
			if (currentCandidate == null || currentCandidate.finished == defaultTime)
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
			CandidateEntity currentCandidate = await GetCandidate(ID);
			if (currentCandidate == null || currentCandidate.finished == new DateTime())
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
        /// Creates a dictionary consisting of all statistics of a candidate.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the data.</returns>
        async public static Task<Dictionary<int,Dictionary<string,int>>> MakeStatisticsCandidate(string ID)
        {
			CandidateEntity candidate = await GetCandidate(ID);
			if(candidate == null || !candidate.IsFinished())
			{
				return null;
			}
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = GetStatisticsFunctions();
            Dictionary<int,Dictionary<string,int>> dataCandidate = new Dictionary<int,Dictionary<string,int>>();
            for (int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
                LevelSession levelSession = candidate.GetLevelSession(levelNumber);
                if(!levelSession.Solved)
                {
                    continue;
                }
                Dictionary<string,int> dataSingleLevel = new Dictionary<string, int>();
                foreach(KeyValuePair<string,Func<LevelSession,int>> nameDataAndFunctionToIntPair in statisticsFunctions){
                    dataSingleLevel.Add(nameDataAndFunctionToIntPair.Key, nameDataAndFunctionToIntPair.Value(levelSession));
                }
                dataCandidate.Add(levelNumber,dataSingleLevel);
                
            }
            return dataCandidate;
        }
		
        /// <summary>
        /// Creates a dictionary consisting of all statistics of all candidates.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
        async public static Task<Dictionary<int,Dictionary<string, Dictionary<int, int>>>> MakeStatisticsEveryone()
        {
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = GetStatisticsFunctions();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> dataEveryone = new Dictionary<int,Dictionary<string, Dictionary<int, int>>>();
            for (int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
                Dictionary<string,Dictionary<int, int>> dataSingleLevel = new Dictionary<string, Dictionary<int, int>>();
                foreach(KeyValuePair<string,Func<LevelSession,int>> nameDataAndFunctionToIntPair in statisticsFunctions){
                    Func<LevelSession,int> functionStatistic = nameDataAndFunctionToIntPair.Value;
					List<CandidateEntity> listOfCandidatesWithScores = await MongoDB.Collection<CandidateEntity>().AsQueryable()
						.Where(candidate => candidate.finished > new DateTime() && candidate.GameResults[levelNumber-1].Solved)
						.ToListAsync();
                    var pairingScoreAmountCandidates = listOfCandidatesWithScores.GroupBy(candidate => functionStatistic(candidate.GameResults[levelNumber-1]))
						.Select(result => new { Score = result.Key, AmountCandidates = result.Count() })
                        .OrderBy(result => result.Score);
					Dictionary<int,int> levelStatistic = pairingScoreAmountCandidates.ToDictionary(result => result.Score, result => result.AmountCandidates);
                    dataSingleLevel.Add(nameDataAndFunctionToIntPair.Key, levelStatistic);
                }
                dataEveryone.Add(levelNumber, dataSingleLevel);
            }
            return dataEveryone;
        }

        /// <summary>
        /// This method creates a dictionary consisting of the number of candidates that did not solve a certain level.
        /// </summary>
        /// <returns></returns>
        async public static Task<Dictionary<int,int>> NumberOfCandidatesNotSolvedPerLevel()
        {
            Dictionary<int,int> amountUnsolved = new Dictionary<int, int>();
            for(int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
				amountUnsolved[levelNumber] = await MongoDB.Collection<CandidateEntity>().AsQueryable()
					.CountAsync(candidate => candidate.finished > new DateTime() && !candidate.GameResults[levelNumber-1].Solved);
            }
            return amountUnsolved;
        }
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