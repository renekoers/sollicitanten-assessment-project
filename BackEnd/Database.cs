using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using MongoDB.Driver;

namespace BackEnd
{
	public class Database
	{
		internal static IMongoDatabase GetDatabase()
		{
			var client = new MongoClient("mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority");
			var database = client.GetDatabase("sylveon");
			return database;
		}

	}
}