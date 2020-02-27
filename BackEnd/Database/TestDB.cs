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
            candidate.ID = TestRepo.Count.ToString();
			TestRepo.Add(candidate.ID, candidate);
			return candidate.ID;
		}

        async public Task SaveCandidate(CandidateEntity candidate)
        {
            await new Task(() => {});
            TestRepo[candidate.ID] = candidate;
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
            return finishedCandidatesBefore.FirstOrDefault();
        }
        async public Task<List<CandidateEntity>> GetListOfCandidatesWithScores(int levelNumber)
        {
            await new Task(() => {});
            return TestRepo.Where(pair => pair.Value.finished > new DateTime() && pair.Value.GameResults[levelNumber-1].Solved).Select(pair => pair.Value).ToList();
        }
        async public Task<int> GetAmountUnsolved(int levelNumber)
        {
            await new Task(() => {});
            return TestRepo.Where(pair => pair.Value.finished > new DateTime() && !pair.Value.GameResults[levelNumber-1].Solved).Count();
        }
    }
}