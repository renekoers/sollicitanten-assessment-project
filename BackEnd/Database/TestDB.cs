using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd {

    public class TestDB : IRepository 
    {
        internal Dictionary<string, CandidateEntity> TestRepo = new Dictionary<string, CandidateEntity>();
        async public Task<string> ValidateHR(string name)
        {
            await new Task(() => {});
            return "hashedPassword";
        }

        async public Task<string> AddCandidate(string name)
		{
            await new Task(() => {});
			CandidateEntity candidate = new CandidateEntity(name);
            string ID = TestRepo.Count.ToString();
			TestRepo.Add(ID, candidate);
			return ID;
		}

        async public Task SaveCandidate(CandidateEntity candidate)
        {
            await new Task(() => {});
            TestRepo[candidate.Name] = candidate;
        }

        async public Task<IState> GetTutorialLevel()
		{
            await new Task(() => {});
			return new State(new Puzzle(Level.Get(0)));
		}

        async public Task<CandidateEntity> GetCandidate(string ID)
        {
            await new Task(() => {});
            if(TestRepo.TryGetValue(ID, out CandidateEntity candidate))
            {
                return candidate;
            } 
            else
            {
                return null;
            }
        }

        async public Task<List<CandidateEntity>> GetCandidates()
        {
            await new Task(() => {});
            return TestRepo.Where(pair => pair.Value.started == new DateTime()).Select(pair => pair.Value).ToList();
        }

        async public Task<List<CandidateEntity>> GetCandidatesAfterTime(DateTime dateTime)
        {
            await new Task(() => {});
            return TestRepo.Where(pair => pair.Value.finished > dateTime).Select(pair => pair.Value).ToList();
        }

        async public Task<CandidateEntity> GetLastCandidateBefore(DateTime dateTime)
        {
            await new Task(() => {});
            List<CandidateEntity> finishedCandidatesBefore = TestRepo.Where(pair => pair.Value.finished < dateTime && pair.Value.IsFinished())
                                                                    .Select(pair => pair.Value).OrderByDescending(candidate => candidate.finished).ToList();
            return finishedCandidatesBefore.Count>0 ? finishedCandidatesBefore.First() : null;
        }

        async public Task<Dictionary<int,Dictionary<string, Dictionary<int, int>>>> GetStatisticsEveryone()
        {
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = GetStatisticsFunctions();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> dataEveryone = new Dictionary<int,Dictionary<string, Dictionary<int, int>>>();
            for (int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
                Dictionary<string,Dictionary<int, int>> dataSingleLevel = new Dictionary<string, Dictionary<int, int>>();
                foreach(KeyValuePair<string,Func<LevelSession,int>> nameDataAndFunctionToIntPair in statisticsFunctions){
                    Func<LevelSession,int> functionStatistic = nameDataAndFunctionToIntPair.Value;
					List<CandidateEntity> listOfCandidatesWithScores = await GetCandidatesThatSolvedLevel(levelNumber);
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
        async public Task<List<CandidateEntity>> GetCandidatesThatSolvedLevel(int levelNumber)
        {
            await new Task(() => {});
            return TestRepo.Where(pair => pair.Value.finished > new DateTime() && pair.Value.GameResults[levelNumber-1].Solved).Select(pair => pair.Value).ToList();
        }

        async public Task<Dictionary<int,int>> GetStatisticsUnsolved()
        {
            Dictionary<int,int> amountUnsolved = new Dictionary<int, int>();
            for(int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
				amountUnsolved[levelNumber] = await GetAmountCandidatesThatNotSolvedLevel(levelNumber);
            }
            return amountUnsolved;
        }
        async public Task<int> GetAmountCandidatesThatNotSolvedLevel(int levelNumber)
        {
            await new Task(() => {});
            return TestRepo.Where(pair => pair.Value.finished > new DateTime() && !pair.Value.GameResults[levelNumber-1].Solved).Count();
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