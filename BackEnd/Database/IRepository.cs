using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BackEnd
{
    public interface IRepository
    {
        Task<string> ValidateHR(string name);

        Task<string> AddCandidate(string name);

        Task SaveCandidate(CandidateEntity candidate);

        Task<Level> GetTutorialLevel();

        Task<CandidateEntity> GetCandidate(string ID);

        Task<List<CandidateEntity>> GetCandidates();

        Task<List<CandidateEntity>> GetCandidatesAfterTime(DateTime dateTime);

        Task<CandidateEntity> GetLastCandidateBefore(DateTime dateTime);

        Task<Dictionary<int,Dictionary<string, Dictionary<int, int>>>> GetStatisticsEveryone();

        Task<Dictionary<int,int>> GetStatisticsUnsolved();
    }
}
