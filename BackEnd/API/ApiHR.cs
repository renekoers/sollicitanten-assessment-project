using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		MongoDataBase MDB = new MongoDataBase();
		async public Task<bool> AddCandidate(string name)
		{
			return await MDB.AddCandidate(name) != null;
		}
    }
}