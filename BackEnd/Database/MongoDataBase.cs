using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace BackEnd {

    public class MongoDataBase : IRepository
    {
        
        private static DB MongoDB = new DB(MongoClientSettings.FromConnectionString(
		"mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority"),
		"sylveon");

        /// <summary>
		/// This method should validate the credentials of HR. This will later be implemented!!!!!!
		/// </summary>
		/// <returns></returns>
        async public Task<string> ValidateHR(string name){
            await;
            return "hashedPassword";
        }

        async public Task<string> AddCandidate(string name)
		{
			CandidateEntity candidate = new CandidateEntity(name);
			await MongoDB.SaveAsync(candidate);
			return candidate.ID;
		}

        async public Task SaveCandidate(CandidateEntity candidate){
            await MongoDB.SaveAsync(candidate);
        }

        async public Task<IState> GetTutorialLevel(){
            return new State(new Puzzle(Level.Get(0)));
        }

        async public Task<CandidateEntity> GetCandidate(string ID){
            try
			{
				return await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			} catch(Exception error)
			{
				Console.WriteLine("Can't find any user with ID: " + ID);
				Console.WriteLine(error.StackTrace);
				return null;
			}
        }

        async public Task<List<CandidateEntity>> GetCandidates(){
            return (await MongoDB.Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
        }

        async public Task<List<CandidateEntity>> GetCandidatesAfterTime(DateTime dateTime){
            IEnumerable<CandidateEntity> newFinishedCandidates = await MongoDB.Find<CandidateEntity>()
                .Match(candidate => (candidate.finished > dateTime))
                .Sort(candidate => candidate.finished, Order.Ascending)
                .ExecuteAsync();
			return newFinishedCandidates.ToList();
        }


        async public Task<CandidateEntity> GetLastCandidateBefore(DateTime dateTime){
            IEnumerable<CandidateEntity> newFinishedCandidates = await MongoDB.Find<CandidateEntity>()
                .Match(candidate => (candidate.finished < dateTime))
                .Sort(candidate => candidate.finished, Order.Descending)
                .ExecuteAsync();
			return newFinishedCandidates.ToList().First();
        }

        /// <summary>
        /// Creates a dictionary consisting of all statistics of all candidates.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
        async public Task<Dictionary<int,Dictionary<string, Dictionary<int, int>>>> GetStatisticsEveryone(){
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
        async public Task<Dictionary<int,int>> GetStatisticsUnsolved(){
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