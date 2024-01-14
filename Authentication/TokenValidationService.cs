using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DinderMVC.Authentication
{


    public class TokenIssuerService
    {
        public (string Bearer, DateTime Expiration) IssueToken(string appInstallGuid, string userGuid)
        {
          
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("9A7194787ADCB8FC4DCDA94DA11E9ASDVA"); // Replace with a secure key

                DateTime expirationDate = DateTime.UtcNow.AddHours(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim("appInstallGuid", appInstallGuid),
                new Claim("userGuid", userGuid),
                    }),
                    Expires = expirationDate,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "YourIssuer",
                    Audience = "YourAudience"
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                


                return (Bearer: tokenHandler.WriteToken(token), Expiration: expirationDate);
            

        }
    }
}