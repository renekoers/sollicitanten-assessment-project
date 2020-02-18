using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BackEnd
{
	public class Database
	{
		private static DB GetDatabase()
		{
			return new DB(MongoClientSettings.FromConnectionString(
		"mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority"),
		"sylveon");
		}

		async internal static Task<string> AddNewCandidate(string name)
		{
			CandidateEntity candidate = new CandidateEntity(name);
			await GetDatabase().SaveAsync(candidate);
			return candidate.ID;
		}
		async internal static Task<CandidateEntity> GetCandidate()
		{
			List<CandidateEntity> unstartedCandidates = await DB.Find<CandidateEntity>()
				.Match(candidate => candidate.started == new DateTime())
				.Limit(1)
				.ExecuteAsync();
			return unstartedCandidates.Count!=0 ? unstartedCandidates.First() : null;
		}

		async internal static Task<CandidateEntity> GetCandidate(string ID)
		{
			return await DB.Find<CandidateEntity>().OneAsync(ID);
		}
		async internal static Task<List<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return (await GetDatabase().Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
		}
		async internal static Task<bool> IsUnstarted(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			return candidate != null ? candidate.started > new DateTime() : false;
		}
		async internal static Task<bool> StartSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await DB.Update<CandidateEntity>()
				.Match(candidate => candidate.ID == ID && candidate.started == defaultTime)
				.Modify(candidate => candidate.started, DateTime.UtcNow)
				.Modify(candidate => candidate.GameResults, new GameSession())
				.ExecuteAsync();
			CandidateEntity foundCandidate = await GetCandidate(ID);
			return foundCandidate != null && foundCandidate.started > defaultTime;
		}
		async internal static Task<bool> EndSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await DB.Update<CandidateEntity>()
				.Match(candidate => candidate.ID == ID && candidate.started > defaultTime && candidate.finished == defaultTime)
				.Modify(candidate => candidate.finished, DateTime.UtcNow)
				.ExecuteAsync();
			CandidateEntity foundCandidate = await GetCandidate(ID);
			return foundCandidate != null && foundCandidate.finished > defaultTime;
		}

		// The following methods are used by HR to receive IDs of candidates.
		async internal static Task<List<string>> GetFinishedIDsAfterTime(DateTime time)
		{
			IEnumerable<CandidateEntity> newFinishedCandidates = await DB.Find<CandidateEntity>()
                    .Match(candidate => (candidate.finished > time))
                    .Sort(candidate => candidate.finished, Order.Ascending)
                    .ExecuteAsync();
			return newFinishedCandidates.Select(candidate => candidate.ID).ToList();
		}
		async internal static Task<string> GetPreviousFinishedID(string ID)
		{
			CandidateEntity currentCandidate = await GetCandidate(ID);
			if(currentCandidate.finished == new DateTime())
			{
				return null;
			}
			IEnumerable<CandidateEntity> previousFinishedCandidates = await DB.Find<CandidateEntity>()
                    .Match(candidate => (candidate.finished < currentCandidate.finished))
                    .Sort(candidate => candidate.finished, Order.Descending)
                    .ExecuteAsync();
			return previousFinishedCandidates.Count()>0 ? previousFinishedCandidates.First().ID : null;
		}
		async internal static Task<string> GetNextFinishedID(string ID)
		{
			CandidateEntity currentCandidate = await GetCandidate(ID);
			if(currentCandidate.finished == new DateTime())
			{
				return null;
			}
			IEnumerable<CandidateEntity> nextFinishedCandidates = await DB.Find<CandidateEntity>()
                    .Match(candidate => (candidate.finished > currentCandidate.finished))
                    .Sort(candidate => candidate.finished, Order.Ascending)
                    .ExecuteAsync();
			return nextFinishedCandidates.Count()>0 ? nextFinishedCandidates.First().ID : null;
		}
		async internal static Task<string> GetLastFinishedID()
		{
			IEnumerable<CandidateEntity> finishedCandidates = await DB.Find<CandidateEntity>()
                    .Sort(candidate => candidate.finished, Order.Descending)
                    .ExecuteAsync();
			return finishedCandidates.Count()>0 ? finishedCandidates.First().ID : null;
		}
	}
}