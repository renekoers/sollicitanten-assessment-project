using BackEnd;
using Microsoft.AspNetCore.Mvc;
using JSonWebToken;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/HR"), Authorize]
	public class CredentialsController : Controller
	{

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
			if (await Api.AddCandidate(name))
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}

		}
		[HttpGet("newFinished")]
		public ActionResult<List<string>> GetNewFinished()
		{
			ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
			long time = long.Parse(identity.FindFirst("Time").Value);
			List<string> newFinishedIDs = Api.GetFinishedIDsAfterEpochTime(time);
			if (newFinishedIDs.Count == 0)
			{
				return NotFound();
			}
			else
			{
				return newFinishedIDs;
			}
		}
	}
}