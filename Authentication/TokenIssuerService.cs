using DinderMVC.Models;
using System.Linq;

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