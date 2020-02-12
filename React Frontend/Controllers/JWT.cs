using System;
using BackEnd;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;

namespace JSonWebToken
{
    internal class JWT
    {
        internal static readonly string Issuer = "Project-Sylveon-scrt";
        internal static readonly string Audience = "Project-Sylveon-scrt";
        internal static readonly string Scrt = "Project-Sylveon-scrt";
        private static int _expiryInHours = 8;

        internal static string CreateToken(string username, string password)
        {
            if(Api.ValidateUser(username, password)){
                
                var claims = new List<Claim>  
                {  
                    new Claim(JwtRegisteredClaimNames.Sub, username),  
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Time", ""+Api.GetEpochTime()) 
                };
                return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(  
                         issuer: Issuer,  
                         audience: Audience,  
                         claims: claims,  
                         expires: DateTime.UtcNow.AddHours(_expiryInHours),  
                         signingCredentials: new SigningCredentials(  
                                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWT.Scrt)),  
                                        SecurityAlgorithms.HmacSha256)));
            } else {
			    return null;
            }
        }

    }
}
