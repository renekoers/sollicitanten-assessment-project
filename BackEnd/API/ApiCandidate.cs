using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		async public static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return await Database.GetAllUnstartedCandidate();
		}
    }
}