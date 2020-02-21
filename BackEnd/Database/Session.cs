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
		/// <summary>
		/// Checks if a session is valid.
		/// </summary>
		/// <param name="candidate"></param>
		/// <returns>Returns true if the candidate is not null, candidate is started but not finished.!-- MUST INSERT CHECK TIME LATER!!!!!!!!</returns>
		private static bool SessionIsValid(CandidateEntity candidate)
		{
			DateTime defaultTime = new DateTime();
			return candidate != null && candidate.started > defaultTime && candidate.finished == defaultTime;
		}
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
		async internal static Task<LevelSession[]> GetAllLevelSessions(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			if(candidate == null){
				return null;
			}
			return candidate.GameResults;
		}
		/// Changing progress of a levelSession
		async internal static Task<bool> StartLevel(string ID, int levelNumber)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			if(!SessionIsValid(candidate) || candidate.GameResults == null){
				return false;
			}
			LevelSession levelSession = candidate.GetLevelSession(levelNumber);
			if(levelSession == null)
			{
				return false;
			}
			levelSession.Start();
			await candidate.SaveAsync();
			CandidateEntity foundCandidate = await GetCandidate(ID);
			return foundCandidate.GetLevelSession(levelNumber).InProgress;
		}
		async internal static Task<bool> StopLevel(string ID, int levelNumber)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			if(!SessionIsValid(candidate) || candidate.GameResults == null){
				return false;
			}
			LevelSession levelSession = candidate.GetLevelSession(levelNumber);
			if(levelSession == null)
			{
				return false;
			}
			levelSession.Stop();
			await candidate.SaveAsync();
			CandidateEntity foundCandidate = await GetCandidate(ID);
			return !foundCandidate.GetLevelSession(levelNumber).InProgress;
		}
		/// Saving a solution to a levelsession
		async internal static Task<LevelSolution> SubmitSolution(string ID, int levelNumber, Statement[] statements)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			if(!SessionIsValid(candidate) || candidate.GameResults == null){
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
