using DinderMVC.Models;
using DinderMVC.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DinderMVC.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly DinderContext _dbContext;

        public BasicAuthenticationHandler(
             IOptionsMonitor<BasicAuthenticationOptions> options,
             ILoggerFactory logger,
             UrlEncoder encoder,
             ISystemClock clock,
             DinderContext dbContext) // Inject your database context here
             : base(options, logger, encoder, clock)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                string authHeader = Request.Headers["Authorization"];
                string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                string decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                string[] credentials = decodedCredentials.Split(":", StringSplitOptions.RemoveEmptyEntries);

                if (credentials.Length == 2)
                {
                    // You now have the username and password
                    string username = credentials[0];
                    string password = credentials[1];

                    // Perform authentication logic here...
                    var result = await DapperQueries.AuthenticateUser(username, password);
                    if (result == null)
                    {
                        return AuthenticateResult.Fail("Invalid credentials");
                    }
                    Guid userGuid = (Guid)result;

                    //_dbContext.DinderTokens.Where(x => x.BearerToken == )

                    // If authentication is successful, create a ClaimsPrincipal
                    var identity = new ClaimsIdentity("Basic");



                    //identity.AddClaim(new Claim(ClaimTypes.Name, token.UserGuid.ToString()));
                    //identity.AddClaim(new Claim(ClaimTypes.Hash, token.BearerToken));
                    //identity.AddClaim(new Claim(ClaimTypes.Expiration, token.ExpirationDate.ToString()));
                    //identity.AddClaim(new Claim(ClaimTypes.System, token.AppInstallGuid.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Thumbprint, userGuid.ToString()));




                    var principal = new ClaimsPrincipal(identity);

                    // Return successful authentication result
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid credentials format");
                }
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
