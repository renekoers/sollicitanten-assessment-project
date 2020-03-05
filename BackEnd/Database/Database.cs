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

		private static DB MongoDB = new DB(MongoClientSettings.FromConnectionString(
		"mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority"),
		"sylveon");


		async internal static Task<string> AddNewCandidate(string name)
		{
			CandidateEntity candidate = new CandidateEntity(name);
			await MongoDB.SaveAsync(candidate);
			return candidate.ID;
		}
		async internal static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return (await MongoDB.Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
		}
	}
}