using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;

namespace BackEnd
{
	public partial class Database
	{
		/// Get properties of a certain session
		async internal static Task<LevelSession> GetLevelSession(string ID, int levelNumber)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			if(candidate == null || candidate.GameResults == null)
			{
				return null;
			}
			return candidate.GetLevelSession(levelNumber);
		}
		/// Saving a solution to a levelsession
		async internal static Task<LevelSolution> SubmitSolution(string ID, int levelNumber, Statement[] statements)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			if(candidate == null || !candidate.HasTimeLeft() || candidate.GameResults == null){
				return null;
			}
			LevelSession levelSession = candidate.GetLevelSession(levelNumber);
			if(levelSession == null)
			{
				return null;
			}
			int attempts = levelSession.Solutions.Count();
			LevelSolution solution = levelSession.Attempt(statements);
			await candidate.SaveAsync();
			CandidateEntity foundCandidate = await GetCandidate(ID);
			return (foundCandidate.GetLevelSession(levelNumber).Solutions.Count() == attempts +1) ? solution : null;
		}
	}
}
