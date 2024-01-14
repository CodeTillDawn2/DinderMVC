
using DinderDLL.DataModels;
using DinderDLL.Responses;
using DinderMVC.Authentication;
using DinderMVC.Models;
using DinderMVC.Queries;
using DinderMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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


        // GET
        // api/v1/Token/

        /// <summary>
        /// Retrieves auth token --Untested
        /// </summary>
        /// <param name="appInstallID">App Install Guid (required)</param>
        /// <returns>A response with a party</returns>
        /// <response code="200">Returns the party list</response>
        /// <response code="404">If meal is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [ProducesResponseType(typeof(SingleResponse<PartyDM>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{appInstallID}")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<IActionResult> GetTokenAsync([BindRequired] Guid appInstallID)
        {
            string name = nameof(GetTokenAsync);
            var response = new SingleResponse<DinderToken>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                var authenticatedUser = HttpContext.User;




                TokenIssuerService tokenIssuer = new TokenIssuerService();
                string userGuid = authenticatedUser.Claims.Where(x => x.Type == ClaimTypes.Thumbprint).FirstOrDefault().Value;
                (string,DateTime) token = tokenIssuer.IssueToken(appInstallID.ToString(), userGuid);

                DinderToken newToken = new DinderToken();
                newToken.AppInstallGuid = appInstallID;
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
                response.Model = new DinderToken();
                response.Model.AppInstallGuid = newToken.AppInstallGuid;
                response.Model.IPAddress = newToken.IPAddress;
                response.Model.IssueDate = newToken.IssueDate;
                response.Model.ExpirationDate = newToken.ExpirationDate;
                response.Model.BearerToken = newToken.BearerToken;
                response.Model.UserGuid = newToken.UserGuid;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

    }
}
