using DinderDLL.DataModels;
using DinderDLL.Requests;
using DinderMVC.Models;
using DinderMVC.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DinderDLL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinderDLL.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        /// <param name="appInstallID">App Install Guid (required)</param>
        /// <param name="userID">User Guid (required)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="cookGuid">Cook GUID</param>
        /// <param name="sessionName">Session Name</param>
        /// <param name="sessionMessage">Session Message</param>
        /// <returns>A response with parties the user is invited to or is hosting</returns>
        /// <response code="200">Returns the parties list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResponse<PartyDM>),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPartiesAsync([BindRequired] Guid appInstallID, [BindRequired] Guid userID, int pageSize = 10, int pageNumber = 1, 
            Guid? cookGuid = null, string sessionName = null, string sessionMessage = null)
        {
            string name = nameof(GetPartiesAsync);
            LogMethodInvoked(name);

            var response = new PagedResponse<PartyDM>();

            try
            {

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                var query = DbContext.GetParties(userID, cookGuid, sessionName, sessionMessage);

                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                response.ItemsCount = await query.CountAsync();

                response.Model = await query.Paging(pageSize, pageNumber).ToListAsync();

                response.Message = string.Format("Page {0} of {1}, Total of parties: {2}.", pageNumber, response.PageCount, response.ItemsCount);
                response.detailed = true;
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
        /// <param name="appInstallID">App Install Guid (required)</param>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Settings")]
        [ProducesResponseType(typeof(PagedResponse<PartySettingsViewCO>),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPartySettingsAsync([BindRequired] Guid appInstallID, [BindRequired] int PartyID, [BindRequired] Guid UserGuid)
        {
            string name = nameof(GetPartySettingsAsync);
            var response = new PagedResponse<PartySettingsViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                if (!(await DbContext.UserInParty(PartyID,UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(appInstallID, UserGuid, name);
                    return BadRequest();
                }

                response.Model = await DbContext.GetPartySettingsAsync(PartyID);
                response.PageSize = 100;

                response.ItemsCount = response.Model.Count();

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
        /// <param name="appInstallID">App Install Guid (required)</param>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Invites")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPartyInvitesAsync([BindRequired] Guid appInstallID, [BindRequired] int PartyID, [BindRequired] Guid UserGuid)
        {
            string name = nameof(GetPartyInvitesAsync);
            var response = new PagedResponse<PartyInviteViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                if (!(await DbContext.UserInParty(PartyID, UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(appInstallID, UserGuid, name);
                    return BadRequest();
                }

                response.Model = await DbContext.GetPartyInvitesAsync(PartyID);
                response.PageSize = 100;

                response.ItemsCount = response.Model.Count();

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
        /// <param name="appInstallID">App Install Guid (required)</param>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <returns>A response with a party</returns>
        /// <response code="200">Returns the party list</response>
        /// <response code="404">If meal is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPartyAsync([BindRequired] Guid appInstallID, [BindRequired] int PartyID, [BindRequired] Guid UserGuid)
        {
            string name = nameof(GetPartyAsync);
            var response = new SingleResponse<PartyDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                if (!(await DbContext.UserInParty(PartyID, UserGuid)))
                {
                    LogGatekeeperInfraction_NotInvited(appInstallID, UserGuid, name);
                    return BadRequest();
                }

                // Get the party by id
                PartyDM party = await DbContext.GetDetailedPartyByIDAsync(new Party(PartyID));
                if (party != null)
                    response.Model = party;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

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
        /// <param name="appInstallID">App Install Guid</param>
        /// <returns>A response with new meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPartyAsync([BindRequired] Guid appInstallID, [FromBody] PostPartyRequest request)
        {
            string name = nameof(PostPartyAsync);
            var response = new SingleResponse<PartyDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity();

                // Add entity to repository
                DbContext.Parties.Add(entity);

                //Meals
                List<UserMeal> userMeals = DbContext.UserMeals
                    .Where(x => x.CookGuid == request.userGUID).ToList();
                foreach (UserMeal meal in userMeals)
                {
                    PartyMeal partyMeal = new PartyMeal(entity.PartyID,  meal.MealID);
                    entity.Meals.Add(partyMeal);
                }

                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // PUT
        // api/v1/Parties/PartyID/Settings/SettingID

        /// <summary>
        /// Updates an existing party setting --Untested
        /// </summary>
        /// <param name="AppInstallID">App Install Guid (required)</param>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <param name="PartyID">Party ID (required)</param>
        /// <param name="SettingID">Setting ID to update (required)</param>
        /// <param name="ChoiceID">Choice ID (required)</param>
        /// <param name="ChoiceEntry">Choice Entry (optional)</param>
        /// <returns>A response as update party setting result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}/Settings/{SettingID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPartySettingAsync([BindRequired] Guid AppInstallID, [BindRequired] Guid UserGuid, [BindRequired] int PartyID,
            [BindRequired] int SettingID, [BindRequired] int ChoiceID, string ChoiceEntry)
        {
            string name = nameof(PutPartySettingAsync);
            var response = new PagedResponse<PartySettingsViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(AppInstallID)))
                {
                    LogInvalidInstall(AppInstallID, name);
                    return BadRequest();
                }
                if (!(await DbContext.UserIsHost(PartyID, UserGuid)))
                {
                    LogGatekeeperInfraction_NotHost(AppInstallID, UserGuid, name);
                    return BadRequest();
                }
                List<PartySettingsViewCO> model = new List<PartySettingsViewCO>();

         
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

                response.Model = await DbContext.GetPartySettingsAsync(PartyID);



            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // PUT
        // api/v1/Users/Meals/5

        /// <summary>
        /// Updates an existing party --Untested
        /// </summary>
        /// <param name="PartyID">Party ID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPartyAsync(int PartyID, [FromBody] PutPartyRequest request)
        {
            string name = nameof(PutPartyAsync);
            var response = new SingleResponse<PartyDM>();



            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetPartyByIDEditableAsync(new Party(PartyID));

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.CookGuid != request.userGUID)
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
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // DELETE
        // api/v1/Users/Parties/5

        /// <summary>
        /// Deletes an existing party --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">userGuid</param>
        /// <param name="PartyID">Party ID</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{PartyID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePartyAsync(Guid appInstallID, Guid userGuid, int PartyID)
        {
            string name = nameof(DeletePartyAsync);
            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetPartyByIDEditableAsync(new Party(PartyID));

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.CookGuid != userGuid)
                    return Forbid();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }



        // PUT
        // api/v1/Users/Meals/5

        /// <summary>
        /// Updates an existing party invite --Untested
        /// </summary>
        /// <param name="PartyID">Party ID</param>
        /// <param name="userGuid">Party ID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        /// <response code="200">Returns an instance of ResponseObject</response>
        [HttpPut("{PartyID}/Invites/{userGuid}/")]
        [ProducesResponseType(typeof(PartyInviteDM), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPartyInviteAsync(int PartyID, Guid userGuid, PutPartyInviteRequest request)
        {
            string name = nameof(PutPartyInviteAsync);
            var response = new SingleResponse<PartyInviteDM>();



            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetPartyInviteEditableAsync(new Party(PartyID), new User(userGuid));

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                // Set changes to entity
                entity.AcceptDate = request.AcceptDate;

                // Update entity in repository
                DbContext.Update(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                response.Model = entity.ReturnDM();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // DELETE
        // api/v1/Users/Parties/5

        /// <summary>
        /// Deletes an existing Party Invite --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">userGuid</param>
        /// <param name="partyID">Party ID</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{partyID}/Invites/{userGuid}/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePartyInviteAsync(Guid appInstallID, int partyID, Guid userGuid)
        {
            string name = nameof(DeletePartyInviteAsync);
            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetPartyInviteEditableAsync(new Party(partyID), new User(userGuid));

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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // PUT
        // api/v1/Users/Meals/5

        /// <summary>
        /// Updates an existing party choice --Untested
        /// </summary>
        /// <param name="partyID">Party ID</param>
        /// <param name="userGuid">User GUID</param>
        /// <param name="mealID">Meal ID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update party result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{PartyID}/Choices/{userGuid}/{mealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutPartyChoiceAsync(int partyID, Guid userGuid, int mealID, PutPartyChoiceRequest request)
        {
            string name = nameof(PutPartyChoiceAsync);
            var response = new SingleResponse<PartyChoiceDM>();



            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetPartyChoiceEditableAsync(new Party(partyID), new User(userGuid), new UserMeal(userGuid,mealID));

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
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // DELETE
        // api/v1/Users/Parties/5

        /// <summary>
        /// Deletes an existing Party Choice --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">userGuid</param>
        /// <param name="partyID">Party ID</param>
        /// <param name="mealID">Party ID</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{partyID}/Choices/{userGuid}/{mealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePartyChoiceAsync(Guid appInstallID, int partyID, Guid userGuid, int mealID)
        {
            string name = nameof(DeletePartyChoiceAsync);
            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetPartyInviteEditableAsync(new Party(partyID), new User(userGuid));

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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }
        // POST
        // api/v1/Party/

        /// <summary>
        /// Creates a new party choice --Untested
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
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPartyChoiceAsync(int partyID, Guid userGuid, PostPartyChoiceRequest request)
        {
            string name = nameof(PostPartyChoiceAsync);
            var response = new SingleResponse<PartyChoiceDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }



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
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }


        // GET
        // api/v1/Party/PartyID/Invites

        /// <summary>
        /// Retrieves party choices --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="PartyID">PartyID</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Choices")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPartyChoicesAsync(Guid appInstallID, int PartyID, Guid UserGuid)
        {
            string name = nameof(GetPartyChoicesAsync);
            var response = new PagedResponse<PartyChoiceDM>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                response.Model = await DbContext.GetPartyChoicesAsync(PartyID, UserGuid);
                response.PageSize = 100;


                response.ItemsCount = response.Model.Count();

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
        /// Creates a new party --Untested
        /// </summary>
        /// <param name="partyID">Request model</param>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{partyID}/Invites/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPartyInviteAsync(int partyID, PostPartyInviteRequest request)
        {
            string name = nameof(PostPartyChoiceAsync);
            var response = new SingleResponse<PartyInviteDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }



                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(partyID);

                // Add entity to repository
                DbContext.PartyInvites.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // GET
        // api/v1/Party/PartyID/Meals

        /// <summary>
        /// Retrieves party meals --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="partyID">PartyID</param>
        /// <returns>A response with party settings list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{PartyID}/Meals")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPartyMealsAsync(Guid appInstallID, int partyID)
        {
            string name = nameof(GetPartyMealsAsync);
            var response = new PagedResponse<MealDM>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                response.Model = await DbContext.GetPartyMealsAsync(partyID);
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
        /// Creates a new party meal --Untested
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="201">A response as creation of party</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{PartyID}/Meals/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostPartyMealAsync(PostPartyMealRequest request)
        {
            string name = nameof(PostPartyMealAsync);
            var response = new SingleResponse<PartyMealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity();

                // Add entity to repository
                DbContext.PartyMeals.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }
        // DELETE
        // api/v1/Users/Parties/5

        /// <summary>
        /// Deletes an existing Party Meal --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">userGuid</param>
        /// <param name="partyID">Party ID</param>
        /// <param name="MealID">Party ID</param>
        /// <returns>A response as delete party result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{PartyID}/Meals/{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePartyMealAsync(Guid appInstallID, int partyID, int MealID, Guid userGuid)
        {
            string name = nameof(DeletePartyMealAsync);
            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetPartyMealEditableAsync(partyID, MealID);

                var party = await DbContext.GetPartyByIDEditableAsync(new Party(partyID));

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (party.CookGuid != userGuid)
                    return Forbid();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();
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
