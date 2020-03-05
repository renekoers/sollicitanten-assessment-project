using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEnd;

namespace React_Frontend.Controllers
{
    [ApiController]
    [Route("api/candidate")]
    public class CandidateController : Controller
    {
        private IRepository _repo;
		public CandidateController(IRepository repo = null) : base()
		{
			_repo = repo == null ? new MongoDataBase() : repo;
		}
        
        /// <summary>
        /// Finds the candidate that belongs to the id.
        /// </summary>
        /// <returns> Returns the candidate entity.
		/// Returns Not Found if the ID is invalid.</returns>
        [HttpGet("{id}")]
        async public Task<ActionResult<string>> getCandidateName(string id)
        {
            CandidateEntity candidate = await _repo.GetCandidate(id);
            if (candidate == null)
            {
                return NotFound();
            }
            else
            {
                return JSON.Serialize(candidate);
            }

        }
        
        /// <summary>
        /// Returns the status of the candidate.
        /// </summary>
        /// <returns> Returns a JSON with fields started and finished.
		/// Returns Not Found if the ID is invalid.</returns>
        [HttpGet("status")]
        async public Task<ActionResult<string>> getStatus([FromHeader(Name="Authorization")] string sessionID)
        {
            CandidateEntity candidate = await _repo.GetCandidate(sessionID);
            if(candidate == null)
            {
                return NotFound();
            }
            else
            {
                return JSON.Serialize(new {started = candidate.IsStarted(), finished = candidate.IsFinished()});
            }
        }

        /// NEEDS TO BE MOVED TO HR CONTROLLER
        [HttpGet("getUnstartedCandidates"), Authorize]
        async public Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidates()
        {
            return await Api.GetAllUnstartedCandidate();
        }
    }
}
