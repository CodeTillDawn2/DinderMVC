using DinderMVC.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DinderMVC.Authentication
{


    public class TokenValidationService
    {
        private readonly DinderContext _dbContext;

        public TokenValidationService(DinderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ValidateToken(string bearerToken)
        {

            var tokenExists = _dbContext.DinderTokens.Any(t => t.BearerToken == bearerToken);
            return tokenExists;
        }
    }
}