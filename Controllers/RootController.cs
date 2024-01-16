
using DinderDLL.DataModels;
using DinderDLL.Models;
using DinderDLL.Responses;
using DinderMVC.Models;
using DinderMVC.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591

    [ApiController]
    [Route("api")]
    public class RootController : DinderControllerBase<RootController>, RootInterface
    {







        public RootController(ILogger<RootController> logger, DinderContext dbContext) : base(logger, dbContext)
        {

        }

#pragma warning restore CS1591


        // GET
        // api

        /// <summary>
        /// Retrieves API versions
        /// </summary>
        /// <returns>Returns links to different versions</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetVersions()
        {
            string name = nameof(GetVersions);
            LogMethodInvoked(name);

            var response = new PagedResponse<LinkCO>();

            try
            {

                List<LinkCO> ResponseLinks = new List<LinkCO>();
                ResponseLinks.Add(new LinkCO(LinkService.REL_apiversion, LinkService.HREF_versionone, LinkService.CRUD_Get));

                // Get the total rows
                response.ItemsCount = ResponseLinks.Count();
                response.PageNumber = 1;
                response.PageSize = ResponseLinks.Count();
                response.detailed = false;

                response.Model = ResponseLinks;

                LogCustom("The versions have been retrieved successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // GET
        // api

        /// <summary>
        /// Retrieves v1 Nodes
        /// </summary>
        /// <returns>Returns links to different nodes</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("v1")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetVersion1Endpoints()
        {
            string name = nameof(GetVersions);
            LogMethodInvoked(name);

            var response = new PagedResponse<LinkCO>();

            try
            {

                List<LinkCO> ResponseLinks = new List<LinkCO>();
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_token, LinkService.HREF_get_token(), LinkService.CRUD_Get));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_parties, LinkService.HREF_parties, LinkService.CRUD_Get));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_party, LinkService.HREF_parties, LinkService.CRUD_Post));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_users, LinkService.HREF_users, LinkService.CRUD_Get));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_user, LinkService.HREF_users, LinkService.CRUD_Post));

                // Get the total rows
                response.ItemsCount = ResponseLinks.Count();
                response.PageNumber = 1;
                response.PageSize = ResponseLinks.Count();
                response.detailed = false;

                response.Model = ResponseLinks;

                LogCustom("The versions have been retrieved successfully.", name);
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

