using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;

namespace BackEnd
{
	public class Database
	{

		private protected static DB MongoDB = new DB(MongoClientSettings.FromConnectionString(
		"mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority"),
		"sylveon");

		// private protected static DB GetDatabase()
		// {
		// 	return new DB(MongoClientSettings.FromConnectionString(
		// "mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority"),
		// "sylveon");
		// }

		async internal static Task<string> AddNewCandidate(string name)
		{
			CandidateEntity candidate = new CandidateEntity(name);
			await MongoDB.SaveAsync(candidate);
			return candidate.ID;
		}
		async internal static Task<CandidateEntity> GetCandidate()
		{
			List<CandidateEntity> unstartedCandidates = await MongoDB.Find<CandidateEntity>()
				.Match(candidate => candidate.started == new DateTime())
				.Limit(1)
				.ExecuteAsync();
			return unstartedCandidates.Count != 0 ? unstartedCandidates.First() : null;
		}

		async internal static Task<CandidateEntity> GetCandidate(string ID)
		{
			return await MongoDB.Find<CandidateEntity>().OneAsync(ID);
		}
		async internal static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return (await MongoDB.Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
		}

		async internal static Task<bool> HasCandidateNotYetStarted(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			// Checks if candidate exists && candidate is not started && candidate is not finished.
			return candidate != null && candidate.started == new DateTime() && candidate.finished == new DateTime();
		}

		async internal static Task<bool> IsCandidateStillActive(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			// Checks if candidate exists && candidate is started && candidate is not finished.
			return candidate != null && candidate.started != new DateTime() && candidate.finished == new DateTime();
		}
		[Obsolete]
		/// This function checks if a candidate is started instead of unstarted. This method is the same as HasCandidateNotYetStarted
		async internal static Task<bool> IsUnstarted(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			// Checks if candidate exists && candidate is started
			return candidate != null && candidate.started > new DateTime();
		}
		async internal static Task<bool> StartSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await MongoDB.Update<CandidateEntity>()
				.Match(candidate => candidate.ID == ID && candidate.started == defaultTime)
				.Modify(candidate => candidate.started, DateTime.UtcNow)
				.Modify(candidate => candidate.GameResults, CandidateEntity.newGameResults())
				.ExecuteAsync();
			// Checks if the candidate with the given ID exists and is started.
			CandidateEntity foundCandidate = await GetCandidate(ID);
			return foundCandidate != null && foundCandidate.started > defaultTime;
		}
		async internal static Task<bool> EndSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await MongoDB.Update<CandidateEntity>()
				.Match(candidate => candidate.ID == ID && candidate.started > defaultTime && candidate.finished == defaultTime)
				.Modify(candidate => candidate.finished, DateTime.UtcNow)
				.ExecuteAsync();
			// Checks if the candidate with the given ID exists and is finished.
			CandidateEntity foundCandidate = await GetCandidate(ID);
			return foundCandidate != null && foundCandidate.finished > defaultTime;
		}
	}
}