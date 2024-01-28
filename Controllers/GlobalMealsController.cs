using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderDLL.Responses;
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
using System.Threading.Tasks;

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
        /// Retrieves global meals by name or description
        /// </summary>
        /// <param name="appInstallID">App Install Guid (required)</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="mealName">Meal Name</param>
        /// <param name="mealDescription">Meal Description</param>
        /// <returns>A response with global meals list</returns>
        /// <response code="200">Returns the global meals list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResponse<GlobalMealDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetGlobalMealsAsync([BindRequired] Guid appInstallID, int pageSize = 10, int pageNumber = 1,
            string mealName = null, string mealDescription = null)
        {

            string name = nameof(GetGlobalMealsAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<GlobalMealDTO>();

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

                List<GlobalMealDM> list = await query.Paging(pageSize, pageNumber).ToListAsync();

                response.Model = list.ConvertAll(x => x.ReturnDTO());

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
        // api/v1/GlobalMeals/{globalMealGuid}/

        /// <summary>
        /// Retrieves a Global Meal by Global Meal GUID
        /// </summary>
        /// <param name="appInstallID">AppInstallID (required)</param>
        /// <param name="globalMealGuid">Global Meal Guid (required)</param>
        /// <returns>A response with a global meal</returns>
        /// <response code="200">Returns the global meal list</response>
        /// <response code="404">If meal is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{globalMealGuid}")]
        [ProducesResponseType(typeof(GlobalMealDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetGlobalMealAsync([BindRequired] Guid appInstallID, [BindRequired] Guid globalMealGuid)
        {

            string name = nameof(GetGlobalMealAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);
            var response = new SingleResponse<GlobalMealDTO>();

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
                {
                    GlobalMealDM mealDM = meal.ReturnDM();
                    response.Model = meal.ReturnDTO();
                }


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



    }
}
