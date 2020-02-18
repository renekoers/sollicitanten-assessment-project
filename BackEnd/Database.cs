using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Entities;

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
		// private static IMongoCollection<BsonDocument> getCandidateCollection()
		// {
		// 	return Database.GetDatabase().GetCollection<BsonDocument>("candidates");
		// }
		// private static IMongoCollection<BsonDocument> getLevelsCollection()
		// {
		// 	return Database.GetDatabase().GetCollection<BsonDocument>("levels");
		// }
		// private static IMongoCollection<BsonDocument> getHrUsersCollection()
		// {
		// 	return Database.GetDatabase().GetCollection<BsonDocument>("hr-users");
		// }

		async internal static Task<string> AddNewCandidate(string name)
		{
			CandidateEntity candidate = new CandidateEntity(name);
			await GetDatabase().SaveAsync(candidate);
			return candidate.ID;
		}

		async internal static Task<CandidateEntity> GetCandidate(string ID)
		{
			Console.WriteLine("Getting candidate...");
			CandidateEntity candidate = await DB.Find<CandidateEntity>().OneAsync(ID);
			return candidate;
		}

		async internal static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return (await GetDatabase().Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
		}
	}
}