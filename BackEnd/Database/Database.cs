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
			CandidateEntity candidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			return candidate;
		}
		async internal static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return (await MongoDB.Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
		}

		async internal static Task<bool> HasCandidateNotYetStarted(string ID)
		{
			CandidateEntity candidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			if (candidate != null && candidate.started == new DateTime() && candidate.finished == new DateTime())
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		async internal static Task<bool> IsCandidateStillActive(string ID)
		{
			CandidateEntity candidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			if (candidate != null && candidate.started != new DateTime() && candidate.finished == new DateTime())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		async internal static Task<bool> IsUnstarted(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			return candidate != null ? candidate.started > new DateTime() : false;
		}
		async internal static Task<bool> StartSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await MongoDB.Update<CandidateEntity>()
				.Match(candidate => candidate.ID == ID && candidate.started == defaultTime)
				.Modify(candidate => candidate.started, DateTime.UtcNow)
				.ExecuteAsync();
			CandidateEntity foundCandidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			return foundCandidate != null && foundCandidate.started > defaultTime;
		}
		async internal static Task<bool> EndSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await MongoDB.Update<CandidateEntity>()
				.Match(candidate => candidate.ID == ID && candidate.started > defaultTime && candidate.finished == defaultTime)
				.Modify(candidate => candidate.finished, DateTime.UtcNow)
				.ExecuteAsync();
			CandidateEntity foundCandidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			return foundCandidate != null && foundCandidate.finished > defaultTime;
		}
	}
}