using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Bson;
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
			CandidateEntity candidate = await DB.Find<CandidateEntity>().OneAsync(ID);
			return candidate;
		}
		async internal static Task<bool> StartSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await DB.Update<CandidateEntity>()
				.Match(a => a.ID == ID && a.started == defaultTime)
				.Modify(a => a.started, DateTime.UtcNow)
				.ExecuteAsync();
			CandidateEntity candidate = DB.Find<CandidateEntity>().One(ID);
			return candidate != null && candidate.started > defaultTime;
		}
		async internal static Task<bool> EndSession(string ID)
		{
			DateTime defaultTime = new DateTime();
			await DB.Update<CandidateEntity>()
				.Match(a => a.ID == ID && a.started > defaultTime && a.finished == defaultTime)
				.Modify(a => a.finished, DateTime.UtcNow)
				.ExecuteAsync();
			CandidateEntity candidate = DB.Find<CandidateEntity>().One(ID);
			return candidate != null && candidate.finished > defaultTime;
		}
		// async internal static Task<List<int>> GetFinishedIDsAfterEpochTime(long epochTime)
		// {
		// 	var newFinishedIds = DB.Find<CandidateEntity>()
        //             .Match(a => a.Age > 30)
        //             .Sort(a => a.Age, Order.Descending)
        //             .Sort(a => a.Name, Order.Ascending)
        //             .Skip(1).Limit(1)
        //             .Project(a => new Author { Name = a.Name })
        //             .Execute();
		// }
	}
}