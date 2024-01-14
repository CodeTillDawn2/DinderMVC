using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace DinderMVC.Authentication
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly List<string> _validTokens;

        public CustomJwtBearerEvents(List<string> validTokens)
        {
            _validTokens = validTokens;
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            var token = context.SecurityToken as JwtSecurityToken;

            if (token != null && !_validTokens.Contains(token.RawData))
            {
                context.Fail("Token validation failed.");
            }

            return Task.CompletedTask;
        }
    }
}
