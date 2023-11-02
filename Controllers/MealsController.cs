using DataModels;
using DinderBackEndv2.Models;
using DinderBackEndv2.Queries;
using DinderBackEndv2.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Requests;
using System;
using System.Threading.Tasks;

namespace DinderBackEndv2.Controllers
{
#pragma warning disable CS1591
    [ApiController]
    [Route("api/v1/[controller]")]
    public partial class MealsController : DinderControllerBase<MealsController>, MealsInterface
    {




        public MealsController(ILogger<MealsController> logger, DinderContext dbContext) : base(logger, dbContext)
        {

        }
#pragma warning restore CS1591

        // GET
        // api/v1/Meals/

        /// <summary>
        /// Retrieves meals
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="userGUID">User GUID</param>
        /// <param name="mealID">Meal ID</param>
        /// <param name="mealName">Meal Name</param>
        /// <param name="mealDescription">Meal Description</param>
        /// <param name="globalLink">Global Link Guid</param>
        /// <param name="madeItBefore">Whether or not they have made it before</param>
        /// <returns>A response with stock items list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMealsAsync(Guid appInstallID, int pageSize = 10, int pageNumber = 1, Guid? userGUID = null,
            int? mealID = null, string mealName = null, string mealDescription = null, Guid? globalLink = null, bool? madeItBefore = null)
        {

            string name = nameof(GetMealsAsync);


            var response = new PagedResponse<MealDM>();

            try
            {

                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                var query = DbContext.GetMeals(userGUID, mealID, mealName, mealDescription, globalLink, madeItBefore);

                response.detailed = false;

                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                response.ItemsCount = await query.CountAsync();

                response.Model = await query.Paging(pageSize, pageNumber).ToListAsync();

                response.Message = string.Format("Page {0} of {1}, Total of meals: {2}.", pageNumber, response.PageCount, response.ItemsCount);

                LogCustom("The meals have been retrieved successfully.", name);
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
        // api/v1/Meals/MealID/

        /// <summary>
        /// Retrieves a meal by MealID
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="id">User GUID</param>
        /// <param name="mealID">Meal ID</param>
        /// <returns>A response with a meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="404">If meal is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMealAsync(Guid appInstallID, int mealID)
        {

            string name = nameof(GetMealAsync);
            var response = new SingleResponse<MealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get the stock item by id
                Meal meal = (await DbContext.GetMealByIDEditableAsync(new Meal(mealID)));

                if (meal != null)
                    response.Model = meal.ReturnDM();

                response.detailed = false;
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
        // api/v1/Meals/

        /// <summary>
        /// Creates a new meal
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="201">A response as creation of meal</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostMealAsync([FromBody] PostMealRequest request)
        {

            string name = nameof(PostMealAsync);

            var response = new SingleResponse<MealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                var existingEntity = await DbContext
                    .GetUserMealByMealNameAsync(new Meal
                    {
                        MealName = request.mealName,
                        UserGUID = request.userGUID
                    });

                if (existingEntity != null)
                    ModelState.AddModelError("MealName", "Meal Name is already taken");

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity();


                DbContext.Meals.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
                response.detailed = false;
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
        /// Updates an existing meal
        /// </summary>
        /// <param name="MealID">Meal ID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update meal result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutMealsAsync(int MealID, [FromBody] PutMealRequest request)
        {
            string name = nameof(PutMealsAsync);
            var response = new SingleResponse<MealDM>();



            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                // Get stock item by id
                var entity = await DbContext.GetMealByIDEditableAsync(new Meal(MealID));

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.UserGUID != request.userGUID)
                    return Forbid();

                // Set changes to entity
                if (request.mealName != null)
                    entity.MealName = request.mealName;
                if (request.mealDescription != null)
                    entity.MealDescription = request.mealDescription;

                if (request.globalLink != null)
                    entity.GlobalLink = request.globalLink;
                entity.MadeItBefore = request.madeItBefore;
                if (request.privateNotes != null)
                    entity.PrivateNotes = request.privateNotes;


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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // DELETE
        // api/v1/Users/Meals/5

        /// <summary>
        /// Deletes an existing meal
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">UserGuid</param>
        /// <param name="mealID">Meal ID</param>
        /// <returns>A response as delete meal result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{mealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteMealAsync(Guid appInstallID, Guid userGuid, int mealID)
        {
            string name = nameof(DeleteMealAsync);

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
                var entity = await DbContext.GetMealByIDEditableAsync(new Meal(mealID));

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.UserGUID != userGuid)
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
