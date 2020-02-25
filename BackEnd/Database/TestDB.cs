using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd {

    public class TestDB : IRepository 
    {
        internal Dictionary<string, CandidateEntity> TestRepo = new Dictionary<string, CandidateEntity>();
        async Task<string> ValidateHR(string name)
        {
            await EmptyPromise();
            return "hashedPassword";
        }

        async Task<string> AddCandidate(string name)
		{
            await EmptyPromise();
			CandidateEntity candidate = new CandidateEntity(name);
            string ID = name;
			TestRepo.Add(ID, candidate);
			return ID;
		}

        async Task SaveCandidate(CandidateEntity candidate)
        {
            await EmptyPromise();
            TestRepo[candidate.Name] = candidate;
        }

        Task<IState> GetTutorialLevel();

        async Task<CandidateEntity> GetCandidate(string ID)
        {
            await EmptyPromise();
            if(TestRepo.TryGetValue(ID, out CandidateEntity candidate))
            {
                return candidate;
            } 
            else
            {
                return null;
            }
        }

        async Task<List<CandidateEntity>> GetCandidates()
        {
            await EmptyPromise();
            // !!!!!!!!!!!!!!!!!!!!!!!!!! Returns CandidateEntities without an ID!!!
            return TestRepo.Where(pair => pair.Value.started == new DateTime()).Select(pair => pair.Value).ToList();
        }

        Task<List<CandidateEntity>> GetCandidatesAfterTime(DateTime dateTime);

        Task<CandidateEntity> GetLastCandidateBefore(DateTime dateTime);

        Task<Dictionary<int,Dictionary<string, Dictionary<int, int>>>> GetStatisticsEveryone();

        Task<Dictionary<int,int>> GetStatisticsUnsolved();

        async private Task EmptyPromise(){
            try
            {
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                await Task.Delay(1);
            }
        }

    }
}