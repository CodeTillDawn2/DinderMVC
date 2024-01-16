using DinderDLL.DataModels;
using DinderDLL.Requests;
using DinderDLL.Responses;
using DinderMVC.Models;
using DinderMVC.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    [ApiController]
    [Route("api/v1/[controller]")]
    public partial class UsersController : DinderControllerBase<UsersController>, UsersInterface
    {


        public UsersController(ILogger<UsersController> logger, DinderContext dbContext) : base(logger, dbContext)
        {

        }
#pragma warning restore CS1591

        // GET
        // api/v1/Users/

        /// <summary>
        /// Retrieves users --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="displayName">Display Name</param>
        /// <param name="createDate">User Created Since</param>
        /// <param name="lastActiveDate">User Active Since</param>
        /// <returns>A response with stock items list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUsersAsync(Guid appInstallID, int pageSize = 10, int pageNumber = 1, string displayName = null, DateTime? createDate = null, DateTime? lastActiveDate = null)
        {

            string name = nameof(GetUsersAsync);

            var response = new PagedResponse<UserDM>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                response.Model = await DapperQueries.GetUsersAsync(pageSize, pageNumber, displayName);

                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                response.ItemsCount = response.Model.Count();

                response.Message = string.Format("Page {0} of {1}, Total of users: {2}.", pageNumber, response.PageCount, response.ItemsCount);

                LogCustom("The users have been retrieved successfully.", name);
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
        // api/v1/Users/

        /// <summary>
        /// Retrieves user friends --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">User Guid</param>
        /// <returns>A response with stock items list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{userGuid}/Friends")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserFriendsAsync(Guid appInstallID, Guid userGuid)
        {
            string name = nameof(GetUserFriendsAsync);

            var response = new PagedResponse<FriendViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                response.Model = await DapperQueries.GetUserFriendsAsync(userGuid);
                response.PageSize = 100;


                response.ItemsCount = response.Model.Count();

                LogCustom("The users have been retrieved successfully.", name);
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
        // api/v1/Parties/

        /// <summary>
        /// Retrieves user Parties --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">User Guid</param>
        /// <returns>A response with stock items list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{userGuid}/Parties")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserPartiesAsync(Guid appInstallID, Guid userGuid)
        {
            string name = nameof(GetUserPartiesAsync);

            var response = new PagedResponse<PartyDM>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }



                response.Model = await DapperQueries.GetUserPartiesAsync(userGuid);
                response.PageSize = 100;
                response.ItemsCount = response.Model.Count();

                LogCustom("The users have been retrieved successfully.", name);
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
        // api/v1/Users/

        /// <summary>
        /// Retrieves a user by GUID --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="UserGuid">User GUID</param>
        /// <returns>A response with a user</returns>
        /// <response code="200">Returns the user list</response>
        /// <response code="404">If user is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{UserGuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserAsync(Guid appInstallID, Guid UserGuid)
        {
            string name = nameof(GetUserAsync);

            var response = new SingleResponse<UserDM>();
            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                response.Model = await DbContext.GetDetailedUserByGuidAsync(new User(UserGuid));
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
        // api/v1/Users/

        /// <summary>
        /// Creates a new user --Untested
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new user</returns>
        /// <response code="200">Returns the user list</response>
        /// <response code="201">A response as creation of user</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostUserAsync([FromBody] PostUserRequest request)
        {
            string name = nameof(PostUserAsync);
            var response = new SingleResponse<UserDM>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                var existingEntity = await DbContext
                    .GetUsersByUsernameAsync(new User
                    {
                        UserName = request.userName
                    });

                if (existingEntity != null)
                    ModelState.AddModelError("UserName", "Username is already taken");

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity();

                // Add entity to repository
                DbContext.Users.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
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
        // api/v1/Users/5

        /// <summary>
        /// Updates an existing user --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">User GUID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update user result</returns>
        /// <response code="200">If user was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{userGuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutUsersAsync(Guid userGuid, [FromBody] PutUserRequest request)
        {
            string name = nameof(PutUsersAsync);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                var entity = await DbContext.GetDetailedUserByGuidAsync(new User(userGuid));

                if (entity == null)
                    return NotFound();

                entity.DisplayName = request.displayName;

                DbContext.Update(entity);

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

        // DELETE
        // api/v1/Users/Users/5

        /// <summary>
        /// Deletes an existing user --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A response as delete user result</returns>
        /// <response code="200">If user was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{UserGuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUserAsync(Guid appInstallID, Guid userGuid)
        {

            string name = nameof(DeleteUserAsync);
            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                var entity = await DbContext.GetDetailedUserByGuidAsync(new User(userGuid));

                if (entity == null)
                    return NotFound();

                DbContext.Remove(entity);

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

        // POST
        // api/v1/Users/{UserGUID}/Friends

        /// <summary>
        /// Creates a user friend --Untested
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <param name="request">Request</param>
        /// <returns>A response as post user result</returns>
        /// <response code="200">If user was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{userGuid}/Friends/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostUserFriendAsync(Guid userGuid, [FromBody] PostUserFriendRequest request)
        {
            string name = nameof(PostUserAsync);
            var response = new SingleResponse<UserFriendDM>();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                var existingEntity = await DbContext
                    .GetUserFriendAsync(new UserFriend
                    {
                        UserGUID = userGuid,
                        FriendGUID = request.friendGUID,
                    });

                if (existingEntity != null)
                    ModelState.AddModelError("UserName", "Username is already taken");

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(userGuid);

                // Add entity to repository
                DbContext.UserFriends.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDM();
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
        // api/v1/Users/5

        /// <summary>
        /// Updates an existing user friend --Untested
        /// </summary>
        /// <param name="userGuid">AppInstallID</param>
        /// <param name="friendGuid">AppInstallID</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update user result</returns>
        /// <response code="200">If user was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{userGuid}/Friends/{friendGuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutUserFriendAsync(Guid userGuid, Guid friendGuid, [FromBody] PutUserFriendRequest request)
        {
            string name = nameof(PutUserFriendAsync);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                var entity = await DbContext.GetUserFriendAsync(new UserFriend(userGuid, friendGuid));

                if (entity == null)
                    return NotFound();

                entity.IsBlocked = request.IsBlocked;

                DbContext.Update(entity);

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

        // Delete
        // api/v1/Users/{UserGUID}/Friends

        /// <summary>
        /// Deletes an existing user friend --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">User GUID</param>
        /// <param name="friendGuid">Freind GUID</param>
        /// <returns>A response as post user result</returns>
        /// <response code="200">If user was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{userGuid}/Friends/{friendGuid}/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUserFriendAsync(Guid appInstallID, Guid userGuid, Guid friendGuid)
        {
            string name = nameof(DeleteUserFriendAsync);
            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                var entity = await DbContext.GetUserFriendAsync(new UserFriend(userGuid, friendGuid));

                if (entity == null)
                    return NotFound();

                DbContext.Remove(entity);

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



        // GET
        // api/v1/Users/{UserGUID}/Meals

        /// <summary>
        /// Retrieves meals --Untested
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
        [HttpGet("{UserGuid}/Meals")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserMealsAsync(Guid appInstallID, Guid userGUID, int pageSize = 10, int pageNumber = 1,
            int? mealID = null, string mealName = null, string mealDescription = null, Guid? globalLink = null, bool? madeItBefore = null)
        {

            string name = nameof(GetUserMealsAsync);


            var response = new PagedResponse<UserMealDM>();

            try
            {

                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                var query = DbContext.GetUserMeals(userGUID, mealID, mealName, mealDescription, globalLink, madeItBefore);

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
        // api/v1/Users/{UserGUID}/Meals/{MealID}

        /// <summary>
        /// Retrieves a meal by MealID --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="mealID">Meal ID</param>
        /// <returns>A response with a meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="404">If meal is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{UserGuid}/Meals/{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserMealAsync(Guid appInstallID, Guid UserGuid, int mealID)
        {

            string name = nameof(GetUserMealAsync);
            var response = new SingleResponse<UserMealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get the stock item by id
                UserMeal meal = (await DbContext.GetUserMealByIDEditableAsync(new UserMeal(UserGuid, mealID)));

                if (meal != null)
                    response.Model = meal.ReturnDM();

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
        // api/v1/Users/{UserGUID}/Meals

        /// <summary>
        /// Creates a new meal --Untested
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new meal</returns>
        /// <response code="200">Returns the meal list</response>
        /// <response code="201">A response as creation of meal</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{UserGuid}/Meals")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostUserMealAsync([FromBody] PostUserMealRequest request)
        {

            string name = nameof(PostUserMealAsync);

            var response = new SingleResponse<UserMealDM>();

            try
            {
                LogMethodInvoked(name);


                if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
                {
                    LogInvalidInstall(request.appInstallID, name);
                    return BadRequest();
                }


                var existingEntity = await DbContext
                    .GetUserMealByMealNameAsync(new UserMeal
                    {
                        MealName = request.mealName,
                        CookGuid = request.userGUID
                    });

                if (existingEntity != null)
                    ModelState.AddModelError("MealName", "Meal Name is already taken");

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity();


                DbContext.UserMeals.Add(entity);

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


        // DELETE
        // api/v1/Users/{UserGUID}/Meals/{mealID}

        /// <summary>
        /// Deletes an existing meal --Untested
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="userGuid">UserGuid</param>
        /// <param name="mealID">Meal ID</param>
        /// <returns>A response as delete meal result</returns>
        /// <response code="200">If meal was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{UserGuid}/Meals/{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUserMealAsync(Guid appInstallID, Guid userGuid, int mealID)
        {
            string name = nameof(DeleteUserMealAsync);

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
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // PUT
        // api/v1/Users/Meals/5

        /// <summary>
        /// Updates an existing meal --Untested
        /// </summary>
        /// <param name="MealID">Meal ID</param>
        /// <param name="userGuid">userGuid</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update meal result</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{UserGuid}/Meals/{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutUserMealsAsync(Guid userGuid, int MealID, [FromBody] PostUserMealRequest request)
        {
            string name = nameof(PutUserMealsAsync);
            var response = new SingleResponse<UserMealDM>();



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
                response.ErrorMessage = "There was an internal error, please contact technical support.";
                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

    }
}
