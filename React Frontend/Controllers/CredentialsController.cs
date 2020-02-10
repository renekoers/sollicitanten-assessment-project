using BackEnd;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using JSonWebToken;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;  

namespace React_Frontend.Controllers
{ 
	[ApiController]
	[Route("api/credentials")]
	public class CredentialsController : Controller
	{
        
		[HttpPost("login")]
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
            string token = JWT.CreateToken(username,password);
            if(token != null){
                return Ok(JSON.Serialize("Bearer " + token));
            } else {
                return Unauthorized();
            } 
		}
        [HttpGet("validate"), Authorize]
        public ActionResult validateToken()
        {
            return Ok();
        }
	}
}