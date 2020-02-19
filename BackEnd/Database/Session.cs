using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;

namespace BackEnd
{
	public class Session : Database
	{
		async internal static Task<bool> StartLevel(string ID, int levelNumber)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			if(candidate == null || candidate.started == new DateTime() || candidate.GameResults == null){
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
	}
}
