using DinderDLL.DataModels;
using DinderDLL.Requests;
using DinderDLL.Responses;
using DinderMVC.Models;
using DinderMVC.Queries;
using DinderMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    [ApiController]
    [Route("api/v1/[controller]")]
    public partial class PartiesController : DinderControllerBase<PartiesController>, PartiesInterface
    {



        public PartiesController(ILogger<PartiesController> logger, DinderContext dbContext) : base(logger, dbContext)
        {

        }
#pragma warning restore CS1591

        // GET
        // api/v1/Party/

        /// <summary>
        /// Retrieves parties the user is invited to or hosting
        /// </summary>
        /// <param name="IsDetailed">Whether or not to include: Party invites, settings, or choices</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="cookGuid">Cook GUID</param>
        /// <param name="sessionName">Session Name</param>
        /// <param name="sessionMessage">Session Message</param>
        /// <returns>A response with parties the user is invited to or is hosting</returns>
        /// <response code="200">Returns the parties list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResponse<PartyDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartiesAsync(bool IsDetailed = false, int pageSize = 10, int pageNumber = 1,
            Guid? cookGuid = null, string sessionName = null, string sessionMessage = null)
        {
            string name = nameof(GetPartiesAsync);
            LogMethodInvoked(name);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<PartyDM>();

            try
            {

                var query = DbContext.GetParties(IsDetailed, id.UserGuid, cookGuid, sessionName, sessionMessage);

                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                response.ItemsCount = await query.CountAsync();

                response.Model = await query.Paging(pageSize, pageNumber).ToListAsync();

                response.Message = string.Format("Page {0} of {1}, Total of parties: {2}.", pageNumber, response.PageCount, response.ItemsCount);
                response.detailed = IsDetailed;
                LogCustom("The parties have been retrieved successfully.", name);
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
        // api/v1/Party/PartyID/Settings

        /// <summary>
        /// Retrieves party settings
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="IsDetailed">Whether or not to include detailed information</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the party settings list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Settings")]
        [ProducesResponseType(typeof(PagedResponse<PartySettingsViewCO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartySettingsAsync([BindRequired] int PartyID)
        {
            string name = nameof(GetPartySettingsAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<PartySettingsViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.UserInParty(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }

                response.Model = await DapperQueries.GetPartySettingsAsync(PartyID);
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;
                response.PageSize = response.Model.Count;
                response.detailed = false;
                LogCustom("The party settings have been retrieved successfully.", name);
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
        // api/v1/Party/PartyID/Invites

        /// <summary>
        /// Retrieves party invites
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Invites")]
        [ProducesResponseType(typeof(PagedResponse<PartyInviteViewCO>), 200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartyInvitesAsync([BindRequired] int PartyID)
        {
            string name = nameof(GetPartyInvitesAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<PartyInviteViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.UserInParty(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }

                response.Model = await DapperQueries.GetPartyInvitesAsync(PartyID);
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;
                response.PageSize = response.Model.Count;
                response.detailed = false;

                LogCustom("The party settings have been retrieved successfully.", name);
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
        // api/v1/Party/PartyID/

        /// <summary>
        /// Retrieves a party by PartyID
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="IsDetailed">Whether or not to include: Party invites, settings, or choices</param>
        /// <returns>A response with a party</returns>
        /// <response code="200">Returns the party</response>
        /// <response code="404">If meal is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}")]
        [ProducesResponseType(typeof(SingleResponse<PartyDM>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartyAsync([BindRequired] int PartyID, [BindRequired] bool IsDetailed = false)
        {
            string name = nameof(GetPartyAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.UserInParty(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }

                // Get the party by id
                PartyDM party = await DbContext.GetDetailedPartyByIDAsync(PartyID, IsDetailed);
                if (party != null)
                    response.Model = party;

                response.detailed = IsDetailed;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // POST
        // api/v1/Party/

        /// <summary>
        /// Creates a new party
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(SingleResponse<PartyDM>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyAsync([FromBody] PostPartyRequest request)
        {
            string name = nameof(PostPartyAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyDM>();

            try
            {
                LogMethodInvoked(name);

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(id.UserGuid);


                // Add entity to repository
                DbContext.Parties.Add(entity);

                //Meals
                List<UserMeal> userMeals = DbContext.UserMeals
                    .Where(x => x.CookGuid == id.UserGuid).ToList();
                foreach (UserMeal meal in userMeals)
                {
                    PartyMeal partyMeal = new PartyMeal(entity.PartyID, meal.MealID);
                    entity.Meals.Add(partyMeal);
                }

                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
                response.detailed = true;
                
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // PUT
        // api/v1/Parties/PartyID/Settings/SettingID

        /// <summary>
        /// Updates an existing party setting
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="SettingID">Setting ID to update (required)</param>
        /// <param name="ChoiceID">Choice ID (required)</param>
        /// <param name="ChoiceEntry">Choice Entry (optional)</param>
        /// <returns>A response as update party setting result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}/Settings/{SettingID}")]
        [ProducesResponseType(typeof(PagedResponse<PartySettingsViewCO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPartySettingAsync([BindRequired] int PartyID,
            [BindRequired] int SettingID, [BindRequired] int ChoiceID, String ChoiceEntry = null)
        {
            string name = nameof(PutPartySettingAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<PartySettingsViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.UserIsHost(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }


                PartySettingMatrix partySettingMatrix = DbContext.PartySettingMatrices.Where(x => x.PartyID == PartyID && x.SettingID == SettingID).FirstOrDefault();

                if (partySettingMatrix != null)
                {
                    partySettingMatrix.ChoiceID = ChoiceID;
                    partySettingMatrix.ChoiceEntry = ChoiceEntry;
                }
                else
                {
                    partySettingMatrix = new PartySettingMatrix(PartyID, SettingID, ChoiceID, ChoiceEntry);
                    DbContext.PartySettingMatrices.Add(partySettingMatrix);

                }


                // Save entity in database
                await DbContext.SaveChangesAsync();

                response.Model = await DapperQueries.GetPartySettingsAsync(PartyID);
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;
                response.PageSize = response.Model.Count;
                response.detailed = false;


            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // PUT
        // api/v1/Users/Meals/5

        /// <summary>
        /// Updates an existing party
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="IsDetailed">Whether or not to include: Party invites, settings, or choices</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}")]
        [ProducesResponseType(typeof(SingleResponse<PartyDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPartyAsync([BindRequired] int PartyID, [FromBody] PutPartyRequest request, [BindRequired] bool IsDetailed = false)
        {
            string name = nameof(PutPartyAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyDM>();



            try
            {
                LogMethodInvoked(name);


                // Get stock item by id
                var entity = await DbContext.GetPartyByIDEditableAsync(PartyID, IsDetailed);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.CookGuid != id.UserGuid)
                    return Forbid();

                // Set changes to entity
                if (request.sessionName != null)
                    entity.SessionName = request.sessionName;
                if (request.sessionMessage != null)
                    entity.SessionMessage = request.sessionMessage;

                // Update entity in repository
                DbContext.Update(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                response.Model = entity.ReturnDM();
                response.detailed = IsDetailed;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // DELETE
        // api/v1/Users/Parties/5

        /// <summary>
        /// Deletes an existing party
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{PartyID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePartyAsync([BindRequired] int PartyID)
        {
            string name = nameof(DeletePartyAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                // Get stock item by id
                var entity = await DbContext.GetPartyByIDEditableAsync(PartyID);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.CookGuid != id.UserGuid)
                    return Forbid();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }



        // PUT
        // api/v1/Users/Meals/5

        /// <summary>
        /// Updates an existing party invite. You can only update your own party invites.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="userGuid">User Guid (required)</param>
        /// <param name="RSVP">Whether you are RSVPing (required)</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        /// <response code="200">Returns an instance of ResponseObject</response>
        [HttpPut("{PartyID}/Invites/{userGuid}/")]
        [ProducesResponseType(typeof(SingleResponse<PartyInviteDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPartyInviteAsync([BindRequired] int PartyID, [BindRequired] Guid userGuid, [BindRequired] bool RSVP)
        {
            string name = nameof(PutPartyInviteAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyInviteDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(userGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }

                // Get stock item by id
                var entity = await DbContext.GetPartyInviteEditableAsync(PartyID, id.UserGuid);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Set changes to entity
                entity.AcceptDate = DateTime.Now;
                entity.RSVP = RSVP;

                // Update entity in repository
                DbContext.Update(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                response.Model = entity.ReturnDM();
                response.detailed = false;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // DELETE
        // api/v1/Users/Parties/5

        /// <summary>
        /// Deletes an existing Party Invite
        /// </summary>
        /// <param name="partyID">Party ID (required)</param>
        /// <param name="userGuid">User Guid (required)</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If party invite was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{partyID}/Invites/{userGuid}/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePartyInviteAsync([BindRequired] int partyID, [BindRequired] Guid userGuid)
        {
            string name = nameof(DeletePartyInviteAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);


                // Get stock item by id
                var entity = await DbContext.GetPartyInviteEditableAsync(partyID, userGuid);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // PUT
        // api/v1/Users/Meals/5

        /// <summary>
        /// Updates an existing party choice
        /// </summary>
        /// <param name="PartyID">Party ID</param>
        /// <param name="userGuid">User GUID</param>
        /// <param name="mealID">Meal ID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}/Choices/{userGuid}/{mealID}")]
        [ProducesResponseType(typeof(SingleResponse<PartyChoiceDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPartyChoiceAsync([BindRequired] int PartyID, [BindRequired] Guid userGuid, [BindRequired] int mealID, [FromBody] PutPartyChoiceRequest request)
        {
            string name = nameof(PutPartyChoiceAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyChoiceDM>();



            try
            {
                if (!(userGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }
                LogMethodInvoked(name);



                // Get stock item by id
                var entity = await DbContext.GetPartyChoiceEditableAsync(PartyID, userGuid, mealID);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Set changes to entity
                entity.SwipeChoice = request.SwipeChoice;

                // Update entity in repository
                DbContext.Update(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                response.Model = entity.ReturnDM();
                response.detailed = false;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }
        
        // POST
        // api/v1/Party/

        /// <summary>
        /// Creates a new party choice
        /// </summary>
        /// <param name="partyID">Request model</param>
        /// <param name="userGuid">Request model</param>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{partyID}/Choices/{userGuid}/")]
        [ProducesResponseType(typeof(SingleResponse<PartyChoiceDM>), 200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyChoiceAsync([BindRequired] int partyID, [BindRequired] Guid userGuid, [FromBody] PostPartyChoiceRequest request)
        {
            string name = nameof(PostPartyChoiceAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyChoiceDM>();

            try
            {
                if (!(userGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }
                LogMethodInvoked(name);

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(partyID, userGuid);

                // Add entity to repository
                DbContext.PartyChoices.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
                response.detailed = false;
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
        // api/v1/Party/PartyID/Invites

        /// <summary>
        /// Retrieves party choices
        /// </summary>
        /// <param name="PartyID">PartyID</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Choices")]
        [ProducesResponseType(typeof(PagedResponse<PartyChoiceDM>), 200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartyChoicesAsync([BindRequired] int PartyID)
        {
            string name = nameof(GetPartyChoicesAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<PartyChoiceDM>();

            try
            {

                LogMethodInvoked(name);


                response.Model = await DapperQueries.GetPartyChoicesAsync(PartyID, id.UserGuid);
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;
                response.PageSize = response.Model.Count;
                response.detailed = false;
                LogCustom("The party settings have been retrieved successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }



        // POST
        // api/v1/Party/

        /// <summary>
        /// Creates a new party invite
        /// </summary>
        /// <param name="partyID">Request model</param>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{partyID}/Invites/")]
        [ProducesResponseType(typeof(SingleResponse<PartyInviteDM>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyInviteAsync([BindRequired] int partyID, [FromBody] PostPartyInviteRequest request)
        {
            string name = nameof(PostPartyChoiceAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyInviteDM>();

            try
            {

                LogMethodInvoked(name);


                if (!ModelState.IsValid)
                    return BadRequest();

                Party party = DbContext.Parties.Where(x => x.PartyID == partyID).FirstOrDefault();
                if (party.CookGuid != id.UserGuid)
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }

                // Create entity from request model
                var entity = request.ToEntity(partyID);
                entity.UserGuid = request.UserGuid;

                // Add entity to repository
                DbContext.PartyInvites.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
                response.detailed = false;
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
        // api/v1/Party/PartyID/Meals

        /// <summary>
        /// Retrieves party meals
        /// </summary>
        /// <param name="PartyID">PartyID</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Meals")]
        [ProducesResponseType(typeof(PagedResponse<UserMealDM>), 200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartyMealsAsync([BindRequired] int PartyID)
        {
            string name = nameof(GetPartyMealsAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<UserMealDM>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.UserInParty(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }

                response.Model = await DapperQueries.GetPartyMealsAsync(PartyID);
                response.PageSize = 100;


                response.ItemsCount = response.Model.Count();

                LogCustom("The party meals have been retrieved successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // POST
        // api/v1/Party/

        /// <summary>
        /// Creates a new party meal
        /// </summary>
        /// <param name="PartyID">Party ID</param>
        /// <param name="MealID">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{PartyID}/Meals/")]
        [ProducesResponseType(typeof(SingleResponse<PartyMealDM>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyMealAsync(int PartyID, int MealID)
        {
            string name = nameof(PostPartyMealAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyMealDM>();

            try
            {
                LogMethodInvoked(name);

                if (!ModelState.IsValid)
                    return BadRequest();

                PartyMeal partyMeal = new PartyMeal();
                partyMeal.PartyID = PartyID;
                partyMeal.MealID = MealID;

                // Add entity to repository
                DbContext.PartyMeals.Add(partyMeal);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = partyMeal.ReturnDM();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }
        // DELETE
        // api/v1/Users/Parties/5

        /// <summary>
        /// Deletes an existing Party Meal
        /// </summary>
        /// <param name="PartyID">Party ID</param>
        /// <param name="MealID">Meal ID</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{PartyID}/Meals/{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePartyMealAsync([BindRequired] int PartyID, [BindRequired] int MealID)
        {
            string name = nameof(DeletePartyMealAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);


                // Get stock item by id
                var entity = await DbContext.GetPartyMealEditableAsync(PartyID, MealID);

                var party = await DbContext.GetPartyByIDEditableAsync(PartyID);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (party.CookGuid != id.UserGuid)
                    return Forbid();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();
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
