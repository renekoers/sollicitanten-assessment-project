using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BackEnd
{
	public class Sessionn : Database
	{
        private static bool SessionInProgress(CandidateEntity candidate)
		{
			DateTime defaultTime = new DateTime();
			// Returns true if session is started && session not finished && the duration of the session is smaller or equal than 20 minutes.
			return candidate.started > defaultTime && candidate.finished == defaultTime && (candidate.finished-candidate.started) <= new TimeSpan(0,20,0);
		}
		async private static Task<bool> StopSessionIfNotInProgress(CandidateEntity candidate)
		{
			if(!SessionInProgress(candidate))
			{
				await Database.EndSession(candidate.ID);
				return true;
			}
			return false;
		}
		async internal static Task<bool> LevelHasBeenStarted(string ID, int levelNumber)
		{
			CandidateEntity candidate = await Database.GetCandidate(ID);
			return candidate != null && candidate.GetLevelSession(levelNumber) != null;
		}
		async internal static Task<bool> StartLevel(string ID, int levelNumber)
		{
			await DB.Update<CandidateEntity>()
				.Match(candidate => candidate.ID == ID && SessionInProgress(candidate) && candidate.GetLevelSession(levelNumber) == null)
				.Modify(candidate => candidate.GameResults[levelNumber-1], new LevelSessionEntity(levelNumber))
				.ExecuteAsync();
			CandidateEntity foundCandidate = await GetCandidate(ID);
			if(foundCandidate == null || await StopSessionIfNotInProgress(foundCandidate))
			{
				return false;
			}
			LevelSessionEntity levelSession = foundCandidate.GetLevelSession(levelNumber);
			return levelSession != null && levelSession.InProgress;
		}
		async internal static Task<bool> ContinueLevel(string ID, int levelNumber)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			// Checking the found candidate if it exists and may continue.
			if(candidate == null || await StopSessionIfNotInProgress(candidate))
			{
				return false;
			}
			LevelSessionEntity levelSession = candidate.GetLevelSession(levelNumber);
			if(levelSession == null)
			{
				return false;
			}
			if(!levelSession.InProgress){
				levelSession.Restart();
				await candidate.SaveAsync();
			}
			return levelSession.InProgress;
		}
	}
}