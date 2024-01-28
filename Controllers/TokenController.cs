using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderDLL.Responses;
using DinderMVC.Authentication;
using DinderMVC.Models;
using DinderMVC.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591

    [ApiController]
    [Route("api/v1/[controller]")]
    public class TokenController : DinderControllerBase<TokenController>, TokenInterface
    {







        public TokenController(ILogger<TokenController> logger, DinderContext dbContext) : base(logger, dbContext)
        {

        }

#pragma warning restore CS1591


        /// <summary>
        /// Retrieves auth token. Uses basic authentication.
        /// </summary>
        /// <param name="AppInstallID">App Install Guid (required)</param>
        /// <returns>A response with a bearer token</returns>
        /// <response code="200">Returns the bearer token and expiration</response>
        /// <response code="400">For a bad request</response>
        /// <response code="404">If app install ID does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [ProducesResponseType(typeof(SingleResponse<DinderTokenDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{AppInstallID}")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<IActionResult> GetTokenAsync([BindRequired] Guid AppInstallID)
        {
            string name = nameof(GetTokenAsync);
            var response = new SingleResponse<DinderTokenDTO>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(AppInstallID)))
                {
                    LogInvalidInstall(AppInstallID, name);
                    return NotFound();
                }

                var authenticatedUser = HttpContext.User;


                TokenIssuerService tokenIssuer = new TokenIssuerService();
                string userGuid = authenticatedUser.Claims.Where(x => x.Type == ClaimTypes.Thumbprint).FirstOrDefault().Value;
                (string, DateTime) token = tokenIssuer.IssueToken(AppInstallID.ToString(), userGuid);

                DinderToken newToken = new DinderToken();
                newToken.AppInstallGuid = AppInstallID;
                newToken.UserGuid = new Guid(userGuid);
                newToken.IPAddress = "";
                newToken.IssueDate = DateTime.Now;
                newToken.ExpirationDate = token.Item2;
                newToken.BearerToken = token.Item1;

                List<DinderToken> oldTokens = DbContext.DinderTokens.Where(x => x.UserGuid == newToken.UserGuid).ToList();
                DbContext.DinderTokens.Add(newToken);
                DbContext.RemoveRange(oldTokens);
                await DbContext.SaveChangesAsync();

                response.detailed = false;
                response.DidError = false;
                response.Model = new DinderTokenDM(newToken.BearerToken, newToken.UserGuid, newToken.ExpirationDate).ReturnDTO();

                LogCustom("The token has been created successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

    }
}
