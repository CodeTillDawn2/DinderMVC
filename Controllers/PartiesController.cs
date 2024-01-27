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


        /// <summary>
        /// Retrieves parties the user is invited to or hosting.
        /// </summary>
        /// <param name="IsDetailed">Whether or not to include: Party invites, settings, or choices</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="cookGuid">Cook GUID</param>
        /// <param name="sessionName">Session Name</param>
        /// <param name="sessionMessage">Session Message</param>
        /// <returns>A response with the parties the user is invited to or is hosting</returns>
        /// <response code="200">Returns the parties list</response>
        /// <response code="400">If the request is bad</response>
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


        /// <summary>
        /// Retrieves party settings. Only the host of a party or an invitee can call this endpoint.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <returns>A response with the party settings list</returns>
        /// <response code="200">Returns the party settings list</response>
        /// <response code="400">If the request is bad</response>
        /// <response code="403">If you are not involved with the party, either as the host or as an invitee</response>
        /// <response code="404">If the party does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Settings")]
        [ProducesResponseType(typeof(PagedResponse<PartySettingsViewCO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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
                    return Forbid();
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

        /// <summary>
        /// Retrieves party invites. Only the host of a party or an invitee can call this endpoint.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the party invites list</response>
        /// <response code="400">If the request is bad</response>
        /// <response code="403">If you are not involved with the party, either as the host or as an invitee</response>
        /// <response code="404">If the party does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Invites")]
        [ProducesResponseType(typeof(PagedResponse<PartyInviteViewCO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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
                    return Forbid();
                }

                response.Model = await DapperQueries.GetPartyInvitesAsync(PartyID);
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;
                response.PageSize = response.Model.Count;
                response.detailed = false;

                LogCustom("The party invites have been retrieved successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        /// <summary>
        /// Retrieves a party by PartyID. Only the host of a party or an invitee can call this endpoint.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="IsDetailed">Whether or not to include: Party invites, settings, or choices</param>
        /// <returns>A response with a party</returns>
        /// <response code="200">Returns the party</response>
        /// <response code="403">If you are not involved with the party, either as the host or as an invitee</response>
        /// <response code="404">If the party does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}")]
        [ProducesResponseType(typeof(SingleResponse<PartyDM>), 200)]
        [ProducesResponseType(403)]
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
                    return Forbid();
                }

                // Get the party by id
                PartyDM party = await DbContext.GetDetailedPartyByIDAsync(PartyID, IsDetailed);
                if (party != null)
                    response.Model = party;

                response.detailed = IsDetailed;

                LogCustom("The party has been retrieved successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Creates a new party. 
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with the new meal</returns>
        /// <response code="201">A successful response</response>
        /// <response code="400">For a bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(SingleResponse<PartyDM>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyAsync([FromBody] PostPartyRequest request)
        {
            string name = nameof(PostPartyAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyDM>();

            try
            {
                LogMethodInvoked(name);

                if (!ModelState.IsValid)
                    return Forbid();

                // Create entity from request model
                var entity = request.ToEntity(id.UserGuid);


                // Add entity to repository
                DbContext.Parties.Add(entity);

                await DbContext.SaveChangesAsync();


                LogCustom("The party has been created successfully.", name);

                List<PartySettingType> settings = DbContext.PartySettingTypes.ToList();

                foreach (PartySettingType setting in settings)
                {
                    PartySettingMatrix matrix = new PartySettingMatrix();
                    matrix.PartyID = entity.PartyID;
                    matrix.SettingID = setting.PartySettingID;
                    matrix.ChoiceID = setting.DefaultSettingChoice;
                    matrix.ChoiceEntry = setting.DefaultSettingEntry;
                    DbContext.PartySettingMatrices.Add(matrix);
                }

                await DbContext.SaveChangesAsync();

                LogCustom("Created " + settings.Count + " settings.", name);

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



        /// <summary>
        /// Updates an existing party setting. Only the host of the party can update a setting.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="SettingID">Setting ID to update (required)</param>
        /// <param name="request">Request body</param>
        /// <returns>A response with the updated party setting</returns>
        /// <response code="200">If the party setting was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are not the host of the party</response>
        /// <response code="404">If the party or the setting does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}/Settings/{SettingID}")]
        [ProducesResponseType(typeof(PagedResponse<PartySettingsViewCO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPartySettingAsync([BindRequired] int PartyID,
            [BindRequired] int SettingID, [FromBody] PutPartySettingRequest request)
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
                    return Forbid();
                }


                PartySettingMatrix partySettingMatrix = DbContext.PartySettingMatrices.Where(x => x.PartyID == PartyID && x.SettingID == SettingID).FirstOrDefault();

                if (partySettingMatrix != null)
                {
                    partySettingMatrix.ChoiceID = request.ChoiceID;
                    partySettingMatrix.ChoiceEntry = request.ChoiceEntry;
                }
                else
                {
                    partySettingMatrix = new PartySettingMatrix(PartyID, SettingID, request.ChoiceID, request.ChoiceEntry);
                    DbContext.PartySettingMatrices.Add(partySettingMatrix);

                }


                // Save entity in database
                await DbContext.SaveChangesAsync();

                response.Model = await DapperQueries.GetPartySettingsAsync(PartyID);
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;
                response.PageSize = response.Model.Count;
                response.detailed = false;

                LogCustom("The party setting has been updated successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Updates an existing party. Only the host of the party can update a party.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="IsDetailed">Whether or not to include: Party invites, settings, meals or choices</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If the party was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are not the host of the party</response>
        /// <response code="404">If the party does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}")]
        [ProducesResponseType(typeof(SingleResponse<PartyDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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

                if (!(await DbContext.UserIsHost(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetPartyByIDEditableAsync(PartyID, IsDetailed);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

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

                LogCustom("The party has been updated successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Deletes an existing party. Only the host of the party can delete a party.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If the party was deleted successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you are not the host of the party</response>
        /// <response code="404">If the party does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{PartyID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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


                if (!(await DbContext.UserIsHost(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetPartyByIDEditableAsync(PartyID);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();

                LogCustom("The party has been deleted successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        /// <summary>
        /// Updates an existing party invite. You can only update your own party invites.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <param name="request">Request body (required)</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If party invite was updated successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If the updated invite is not your invite</response>
        /// <response code="404">If the party or the party invite is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}/Invites/{UserGuid}/")]
        [ProducesResponseType(typeof(SingleResponse<PartyInviteDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPartyInviteAsync([BindRequired] int PartyID, [BindRequired] Guid UserGuid, [FromBody] PutPartyInviteRequest request)
        {
            string name = nameof(PutPartyInviteAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyInviteDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetPartyInviteEditableAsync(PartyID, id.UserGuid);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Set changes to entity
                entity.AcceptDate = DateTime.Now;
                entity.RSVP = request.RSVP;

                // Update entity in repository
                DbContext.Update(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                response.Model = entity.ReturnDM();
                response.detailed = false;

                LogCustom("The party invite has been updated successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Deletes an existing Party Invite. Only the host can delete a party invite.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <returns>A response with no data</returns>
        /// <response code="200">If the party invite was deleted successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you are not the host of the party</response>
        /// <response code="404">If the party or the party invite is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{PartyID}/Invites/{UserGuid}/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePartyInviteAsync([BindRequired] int PartyID, [BindRequired] Guid UserGuid)
        {
            string name = nameof(DeletePartyInviteAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.UserIsHost(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetPartyInviteEditableAsync(PartyID, UserGuid);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();

                LogCustom("The party invite has been deleted successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Updates an existing party choice. You can only update your own party choices and the meal must have been selected for the party.
        /// </summary>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="UserGuid">User GUID (required)</param>
        /// <param name="MealID">Meal ID (required)</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If the party choice was updated successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If this is not your choice or the meal is not selected for the party.</response>
        /// <response code="404">If the party or the party choice is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}/Choices/{UserGuid}/{MealID}")]
        [ProducesResponseType(typeof(SingleResponse<PartyChoiceDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPartyChoiceAsync([BindRequired] int PartyID, [BindRequired] Guid UserGuid, [BindRequired] int MealID, [FromBody] PutPartyChoiceRequest request)
        {
            string name = nameof(PutPartyChoiceAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyChoiceDM>();



            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                if (!await DbContext.MealInParty(PartyID, MealID))
                {
                    LogGatekeeperInfraction_MealNotInParty(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetPartyChoiceEditableAsync(PartyID, UserGuid, MealID);

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

                LogCustom("The party choice has been updated successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        /// <summary>
        /// Creates a new party choice. You can only make choices in parties you are invited to, and only on meals which exist in the party.
        /// </summary>
        /// <param name="PartyID">Request model</param>
        /// <param name="UserGuid">Request model</param>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If this is not your choice or the meal is not selected for the party.</response>
        /// <response code="404">If the party or the party choice is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{PartyID}/Choices/{UserGuid}/")]
        [ProducesResponseType(typeof(SingleResponse<PartyChoiceDM>), 200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyChoiceAsync([BindRequired] int PartyID, [BindRequired] Guid UserGuid, [FromBody] PostPartyChoiceRequest request)
        {
            string name = nameof(PostPartyChoiceAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyChoiceDM>();

            try
            {
                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                if (!await DbContext.MealInParty(PartyID, request.MealID))
                {
                    LogGatekeeperInfraction_MealNotInParty(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }


                LogMethodInvoked(name);

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(PartyID, UserGuid);

                // Add entity to repository
                DbContext.PartyChoices.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
                response.detailed = false;

                LogCustom("The party choice has been created successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Retrieves party choices. You can only see all choices if you are the host. If you are not the host, only your choices will be returned.
        /// </summary>
        /// <param name="PartyID">PartyID</param>
        /// <returns>A response with party choices list</returns>
        /// <response code="200">Returns the party choices list</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you are not the host or an invitee.</response>
        /// <response code="404">If the party is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Choices")]
        [ProducesResponseType(typeof(PagedResponse<PartyChoiceDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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


                if (!(await DbContext.UserInParty(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                response.Model = await DapperQueries.GetPartyChoicesAsync(PartyID, id.UserGuid);
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;
                response.PageSize = response.Model.Count;
                response.detailed = false;
                LogCustom("The party choices have been retrieved successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        /// <summary>
        /// Creates a new party invite. Only the host can create party invites.
        /// </summary>
        /// <param name="PartyID">Request model</param>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="201">A response with the party invite created</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are not the host of the party.</response>
        /// <response code="404">If the party is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{PartyID}/Invites/")]
        [ProducesResponseType(typeof(SingleResponse<PartyInviteDM>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyInviteAsync([BindRequired] int PartyID, [FromBody] PostPartyInviteRequest request)
        {
            string name = nameof(PostPartyChoiceAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<PartyInviteDM>();

            try
            {

                LogMethodInvoked(name);

                if (!(await DbContext.UserIsHost(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                Party party = DbContext.Parties.Where(x => x.PartyID == PartyID).FirstOrDefault();
                if (party.CookGuid != id.UserGuid)
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(PartyID);
                entity.UserGuid = request.UserGuid;

                // Add entity to repository
                DbContext.PartyInvites.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
                response.detailed = false;

                LogCustom("The party invite has been created successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Retrieves party meals. You can only retrieve the party meals if you are the host, or an invitee.
        /// </summary>
        /// <param name="PartyID">PartyID</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are not the host of the party or an invitee.</response>
        /// <response code="404">If the party is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Meals")]
        [ProducesResponseType(typeof(PagedResponse<UserMealDM>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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


        /// <summary>
        /// Creates a new party meal. Only the host can create a party meal.
        /// </summary>
        /// <param name="PartyID">Party ID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="201">A response with the successfully created party meal</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are not the host of the party.</response>
        /// <response code="404">If the party is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{PartyID}/Meals/")]
        [ProducesResponseType(typeof(SingleResponse<PartyMealDM>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostPartyMealAsync([BindRequired] int PartyID, [FromBody] PostPartyMealRequest request)
        {

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            string name = nameof(PostPartyMealAsync);

            var response = new SingleResponse<PartyMealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.UserIsHost(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                if (!ModelState.IsValid)
                    return BadRequest();

                PartyMeal partyMeal = new PartyMeal();
                partyMeal.PartyID = PartyID;
                partyMeal.MealID = request.MealID;

                // Add entity to repository
                DbContext.PartyMeals.Add(partyMeal);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = partyMeal.ReturnDM();

                LogCustom("The party meal has been created successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        /// <summary>
        /// Deletes an existing Party Meal. Only the host can delete a party meal.
        /// </summary>
        /// <param name="PartyID">Party ID</param>
        /// <param name="MealID">Meal ID</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are not the host of the party.</response>
        /// <response code="404">If the party is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{PartyID}/Meals/{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
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

                if (!(await DbContext.UserIsHost(PartyID, id.UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                // Get stock item by id
                var entity = await DbContext.GetPartyMealEditableAsync(PartyID, MealID);

                var party = await DbContext.GetPartyByIDEditableAsync(PartyID);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Remove entity from repository
                DbContext.PartyMeals.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();

                LogCustom("The party meal has been deleted successfully.", name);
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
