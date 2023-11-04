using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DinderDLL.Responses;
using DinderDLL.DataModels;
using DinderMVC.Queries;
using DinderMVC.Models;
using DinderDLL.Requests;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    [ApiController]
    [Route("api/v1/[controller]")]
    public partial class GlobalMealsController : DinderControllerBase<GlobalMealsController>, GlobalMealsInterface
    {




        public GlobalMealsController(ILogger<GlobalMealsController> logger, DinderContext dbContext) : base(logger, dbContext)
        {

        }
#pragma warning restore CS1591

        // GET
        // api/v1/GlobalMeals/

        /// <summary>
        /// Retrieves global meals --untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="mealName">Meal Name</param>
        /// <param name="mealDescription">Meal Description</param>
        /// <returns>A response with stock items list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResponse<GlobalMealDM>),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetGlobalMealsAsync(Guid appInstallID, int pageSize = 10, int pageNumber = 1, 
            string mealName = null, string mealDescription = null)
        {

            string name = nameof(GetGlobalMealsAsync);


            var response = new PagedResponse<GlobalMealDM>();

            try
            {

                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                var query = DbContext.GetGlobalMeals(mealName, mealDescription);

                response.detailed = false;

                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                response.ItemsCount = await query.CountAsync();

                response.Model = await query.Paging(pageSize, pageNumber).ToListAsync();

                response.Message = string.Format("Page {0} of {1}, Total of meals: {2}.", pageNumber, response.PageCount, response.ItemsCount);

                LogCustom("The global meals have been retrieved successfully.", name);
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
        // api/v1/GlobalMeals/{MealGuid}/

        /// <summary>
        /// Retrieves a Global Meal by MealGuid -- Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="globalMealGuid">Global Meal Guid</param>
        /// <returns>A response with a meal</returns>
        /// <response code="200">Returns the global meal list</response>
        /// <response code="404">If meal is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{MealGuid}")]
        [ProducesResponseType(typeof(GlobalMealDM),200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetGlobalMealAsync(Guid appInstallID, Guid globalMealGuid)
        {

            string name = nameof(GetGlobalMealAsync);
            var response = new SingleResponse<GlobalMealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get the stock item by id
                GlobalMeal meal = (await DbContext.GetGlobalMealEditableAsync(globalMealGuid));

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
        // api/v1/GlobalMeals/

        /// <summary>
        /// Creates a new global meal --untested
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="201">A response as creation of meal</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(SingleResponse<GlobalMealDM>),200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostGlobalMealAsync([FromBody] PostGlobalMealRequest request)
        {

            string name = nameof(PostGlobalMealAsync);

            var response = new SingleResponse<GlobalMealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                var existingEntity = await DbContext
                    .GetGlobalMealByName(request.mealName);

                if (existingEntity != null)
                    ModelState.AddModelError("MealName", "Meal Name is already taken");

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity();


                DbContext.GlobalMeals.Add(entity);

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
        /// <param name="userGuid">userGuid</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update meal result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutGlobalMealAsync(Guid userGuid, int MealID, [FromBody] PutGlobalMealRequest request)
        {
            string name = nameof(PutGlobalMealAsync);
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
                var entity = await DbContext.GetUserMealByIDEditableAsync(new UserMeal(userGuid, MealID));

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.CookGuid != request.userGUID)
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
                var entity = await DbContext.GetUserMealByIDEditableAsync(new UserMeal(userGuid, mealID));

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
    }
}
