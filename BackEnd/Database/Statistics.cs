using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BackEnd
{
	public partial class Database
	{
		async internal static Task<List<string>> GetFinishedIDsAfterTime(DateTime time)
		{
			IEnumerable<CandidateEntity> newFinishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Match(candidate => (candidate.finished > time))
					.Sort(candidate => candidate.finished, Order.Ascending)
					.ExecuteAsync();
			return newFinishedCandidates.Select(candidate => candidate.ID).ToList();
		}
		async internal static Task<string> GetPreviousFinishedID(string ID)
		{
			DateTime defaultTime = new DateTime();
			CandidateEntity currentCandidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			if (currentCandidate.finished == defaultTime)
			{
				return null;
			}
			IEnumerable<CandidateEntity> previousFinishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Match(candidate => (candidate.finished < currentCandidate.finished) && (candidate.finished > defaultTime))
					.Sort(candidate => candidate.finished, Order.Descending)
					.ExecuteAsync();
			return previousFinishedCandidates.Count() > 0 ? previousFinishedCandidates.First().ID : null;
		}
		async internal static Task<string> GetNextFinishedID(string ID)
		{
			CandidateEntity currentCandidate = await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			if (currentCandidate.finished == new DateTime())
			{
				return null;
			}
			IEnumerable<CandidateEntity> nextFinishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Match(candidate => (candidate.finished > currentCandidate.finished))
					.Sort(candidate => candidate.finished, Order.Ascending)
					.ExecuteAsync();
			return nextFinishedCandidates.Count() > 0 ? nextFinishedCandidates.First().ID : null;
		}
		async internal static Task<string> GetLastFinishedID()
		{
			IEnumerable<CandidateEntity> finishedCandidates = await MongoDB.Find<CandidateEntity>()
					.Sort(candidate => candidate.finished, Order.Descending)
					.ExecuteAsync();
			return finishedCandidates.Count() > 0 ? finishedCandidates.First().ID : null;
		}
	}
}