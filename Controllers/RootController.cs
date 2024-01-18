
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
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_api));
                ResponseLinks.Add(new LinkCO(LinkService.REL_version_one, LinkService.HREF_versionone));

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

                //Root
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_self, LinkService.HREF_versionone));
                ResponseLinks.Add(new LinkCO(LinkService.REL_apiversion, LinkService.HREF_api));

                //Global meals
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_global_meal, LinkService.HREF_globalmeal()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_global_meals, LinkService.HREF_globalmeals));

                //Parties
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_parties, LinkService.HREF_parties));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_party, LinkService.HREF_parties));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_party_settings, LinkService.HREF_party_settings()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_party_invites, LinkService.HREF_party_invites()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_party, LinkService.HREF_party()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_update_party, LinkService.HREF_party()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_delete_party, LinkService.HREF_party()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_delete_party, LinkService.HREF_party()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_update_party_setting, LinkService.HREF_party_setting()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_update_party_invite, LinkService.HREF_party_invite()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_delete_party_invite, LinkService.HREF_party_invite()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_update_party_choice, LinkService.HREF_party_choice()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_party_choice, LinkService.HREF_party_choices()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_party_choices, LinkService.HREF_party_choices()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_party_invite, LinkService.HREF_party_invites()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_party_meals, LinkService.HREF_party_meals()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_party_meal, LinkService.HREF_party_meal()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_delete_party_meal, LinkService.HREF_party_meal()));

                //Token
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_token, LinkService.HREF_token()));

                //Users
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_users, LinkService.HREF_users));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_user, LinkService.HREF_users));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_user_friends, LinkService.HREF_user_friends()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_user_friend, LinkService.HREF_user_friend()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_user_parties, LinkService.HREF_user_parties()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_user, LinkService.HREF_user()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_delete_user, LinkService.HREF_user()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_update_user, LinkService.HREF_user()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_update_user_friend, LinkService.HREF_user_friend()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_delete_user_friend, LinkService.HREF_user_friend()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_user_meals, LinkService.HREF_user_meals()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_create_user_meal, LinkService.HREF_user_meals()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_get_user_meal, LinkService.HREF_user_meal()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_delete_user_meal, LinkService.HREF_user_meal()));
                ResponseLinks.Add(new LinkCO(LinkService.REL_update_user_meal, LinkService.HREF_user_meal()));






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

