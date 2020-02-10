using BackEnd;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using JSonWebToken;
using Microsoft.AspNetCore.Authorization;  

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
		public ActionResult<string> Login([FromBody] string credentials)
		{
            var creds = JObject.Parse(credentials);
            JToken value = null;
            creds.TryGetValue("username", out value);
            string username = value.ToString();
            creds.TryGetValue("password", out value);
            string password = value.ToString();
            string token = JWT.CreateToken(username,password);
            if(token != null){
                return Ok(JSON.Serialize("Bearer " + token));
            } else {
                return Unauthorized();
            } 
		}
        [HttpGet("test"), Authorize]
        public ActionResult<string> getNumber()
        {
            return Ok(JSON.Serialize("Five"));
        }
	}
}