using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JSonWebToken;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/HR"), Authorize]
	public class HRController : Controller
	{

		private IRepository myDatabase;
		public HRController(){
			this.myDatabase = new MongoDataBase();
		}

		public HRController(IRepository testDB){
			this.myDatabase = new TestDB();
		}
	

		[HttpPost("login"), AllowAnonymous]
		/// <summary>
		/// This method needs to get the authorization of the request. This will be a token to validate HR!!!!!!!!
		/// </summary>
		/// <returns></returns>
		public ActionResult<string> Login([FromBody] JsonElement credentials)
		{
			JsonElement value;
			credentials.TryGetProperty("username", out value);
			string username = value.ToString();
			credentials.TryGetProperty("password", out value);
			string password = value.ToString();
			if (username == null || password == null)
			{
				return BadRequest();
			}
			string token = JWT.CreateToken(username, password);
			if (token != null)
			{
				return Ok(JSON.Serialize("Bearer " + token));
			}
			else
			{
				return Unauthorized();
			}
		}
		[HttpGet("validate")]
		public StatusCodeResult ValidateToken()
		{
			return Ok();
		}
		[HttpPost("candidate")]
		async public Task<StatusCodeResult> AddCandidate([FromBody] string name)
		{
			if(name == "" || name == null){
				return UnprocessableEntity();
			}
			if (await myDatabase.AddCandidate(name) != null)
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}

		}

		/// <summary>
		/// This method creates a list of all candidates that finished a session after a given time
		/// </summary>
		/// <returns>List of Candidates</returns>
		async public Task<List<CandidateEntity>> GetFinishedCandidatesAfterTime(DateTime dateTime)
		{
			return await myDatabase.GetCandidatesAfterTime(dateTime);
		}

		/// <summary>
		/// This method finds the last CandidateEntity that ended the session before the given DateTime.
		/// </summary>
		/// <returns>Candidate if there exists one</returns>
		async public Task<CandidateEntity> GetPreviousFinishedCandidate(DateTime dateTime)
		{
			return await myDatabase.GetLastCandidateBefore(dateTime);
		}

		/// <summary>
		/// This method finds the first CandidateEntity that ended the session after the given ID.
		/// </summary>
		/// <returns>CandidateEntity if there exists one</returns>
		async public Task<CandidateEntity> GetNextFinishedID(string ID)
		{
			return await myDatabase.GetNextFinishedID(ID);
		}

		async public static Task<string> GetLastFinishedID()
		{
			return await Database.GetLastFinishedID();
		}

	}
}