using DinderMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DinderMVC.Authentication
{
    public class BearerAuthenticationHandler : AuthenticationHandler<BearerAuthenticationOptions>
    {

        private DinderContext _dbContext;

        public BearerAuthenticationHandler(
                IOptionsMonitor<BearerAuthenticationOptions> options,
                ILoggerFactory logger,
                UrlEncoder encoder,
                ISystemClock clock,
                DinderContext dbContext)
                : base(options, logger, encoder, clock)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                string authHeader = Request.Headers["Authorization"];
                if (!authHeader.Contains("Bearer "))
                {
                    return AuthenticateResult.Fail("Missing Authorization Header");
                }
                string encodedCredential = authHeader.Substring("Bearer ".Length).Trim();
                string decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredential));
                // Perform token validation logic here...
                DinderToken token = ReturnFoundToken(decodedCredentials);
                if (token != null)
                {

                    User user = _dbContext.Users.Where(x => x.UserGUID == token.UserGuid).FirstOrDefault();

                    // If validation is successful, create a ClaimsPrincipal
                    var identity = new ClaimsIdentity("Bearer");
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.DisplayName));
                    identity.AddClaim(new Claim(ClaimTypes.Hash, token.BearerToken));
                    identity.AddClaim(new Claim(ClaimTypes.Expiration, token.ExpirationDate.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Thumbprint, token.UserGuid.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.System, token.AppInstallGuid.ToString()));
                    var principal = new ClaimsPrincipal(identity);

                    // Return successful authentication result
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid Bearer Token");
                }
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }

        // Example method for token validation - replace with your actual logic
        private DinderToken ReturnFoundToken(string bearerToken)
        {
            DinderToken tokenFound = _dbContext.DinderTokens.Where(x => x.BearerToken == bearerToken).FirstOrDefault();
            return tokenFound;
        }
    }
}
