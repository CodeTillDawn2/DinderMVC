using DinderDLL.DataModels;
using DinderDLL.DTOs;
using DinderDLL.Requests;
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

        /// <summary>
        /// Searches users
        /// </summary>
        /// <param name="PageSize">Page size (optional)</param>
        /// <param name="PageNumber">Page number (optional)</param>
        /// <param name="DisplayName">Display Name (optional)</param>
        /// <returns>A response with user list</returns>
        /// <response code="200">Returns the user list</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResponse<UserDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUsersAsync(int PageSize = 10, int PageNumber = 1, string DisplayName = null)
        {

            string name = nameof(GetUsersAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<UserDTO>();

            try
            {
                LogMethodInvoked(name);

                response.Model = (await DapperQueries.GetUsersAsync(PageSize, PageNumber, DisplayName)).ConvertAll(x => x.ReturnDTO());

                response.PageSize = PageSize;
                response.PageNumber = PageNumber;

                response.ItemsCount = response.Model.Count();

                response.Message = string.Format("Page {0} of {1}, Total of users: {2}.", PageNumber, response.PageCount, response.ItemsCount);

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

        /// <summary>
        /// Retrieves user friends. You can only retrieve your own friends.
        /// </summary>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <returns>A response with your friends list.</returns>
        /// <response code="200">Returns your friends list</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are trying to access someone else's friends list.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{UserGuid}/Friends")]
        [ProducesResponseType(typeof(PagedResponse<FriendViewCO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserFriendsAsync([BindRequired] Guid UserGuid)
        {
            string name = nameof(GetUserFriendsAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<FriendViewCO>();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                response.Model = await DapperQueries.GetUserFriendsAsync(UserGuid);

                response.ItemsCount = response.Model.Count();
                response.PageSize = response.Model.Count();
                response.PageNumber = 1;

                LogCustom("The user friends have been retrieved successfully.", name);
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
        /// Retrieves user Parties. You can only retrieve your own parties.
        /// </summary>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <returns>A response your party list</returns>
        /// <response code="200">Returns your party list</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you are trying to access someone else's party list.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{UserGuid}/Parties")]
        [ProducesResponseType(typeof(PagedResponse<PartyDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserPartiesAsync([BindRequired] Guid UserGuid)
        {
            string name = nameof(GetUserPartiesAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<PartyDTO>();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                response.Model = (await DapperQueries.GetUserPartiesAsync(UserGuid)).ConvertAll(x => x.ReturnDTO());
                response.PageSize = response.Model.Count;
                response.ItemsCount = response.Model.Count;
                response.PageNumber = 1;

                LogCustom("The user parties have been retrieved successfully.", name);
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
        /// Retrieves a user by Guid.
        /// </summary>
        /// <param name="UserGuid">User GUID (required)</param>
        /// <returns>A response with a user</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{UserGuid}")]
        [ProducesResponseType(typeof(SingleResponse<UserDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAsync([BindRequired] Guid UserGuid)
        {
            string name = nameof(GetUserAsync);

            var response = new SingleResponse<UserDTO>();
            try
            {
                LogMethodInvoked(name);

                response.Model = (await DbContext.Users.Where(x => x.UserGUID == UserGuid).FirstOrDefaultAsync()).ReturnDTO();

                LogCustom("The user has been retrieved successfully.", name);
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
        /// Creates a new user. Uses basic authentication.
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new user</returns>
        /// <response code="201">A response with the created user.</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If the username you chose is taken./response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(SingleResponse<UserDTO>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public async Task<IActionResult> PostUserAsync([FromBody] PostUserRequest request)
        {
            string name = nameof(PostUserAsync);
            var response = new SingleResponse<UserDTO>();

            try
            {
                LogMethodInvoked(name);

                var existingEntity = await DbContext
                    .GetUsersByUsernameAsync(request.UserName);

                if (existingEntity != null)
                {
                    ModelState.AddModelError("UserName", "Username is already taken");
                    return Forbid();
                }


                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity();

                // Add entity to repository
                DbContext.Users.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDTO();

                LogCustom("The user has been created successfully.", name);
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
        /// Updates an existing user. You can only update your own user.
        /// </summary>
        /// <param name="UserGuid">User GUID (required)</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update user result</returns>
        /// <response code="200">If the user was updated successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you try to update a user other than your own.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{UserGuid}")]
        [ProducesResponseType(typeof(SingleResponse<UserDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutUsersAsync([BindRequired] Guid UserGuid, [FromBody] PutUserRequest request)
        {
            string name = nameof(PutUsersAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<UserDTO>();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                User user = DbContext.Users.Where(x => x.UserGUID == UserGuid).FirstOrDefault();

                if (user == null)
                    return NotFound();

                user.DisplayName = request.DisplayName;

                await DbContext.SaveChangesAsync();

                response.Model = user.ReturnDTO();

                LogCustom("The user has been updated successfully.", name);
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
        /// Deletes an existing user. You can only delete your own user.
        /// </summary>
        /// <param name="UserGuid">User GUID (required)</param>
        /// <returns>A successful delete response</returns>
        /// <response code="200">If user was deleted successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you tried to delete another user</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{UserGuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUserAsync([BindRequired] Guid UserGuid)
        {

            string name = nameof(DeleteUserAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }


                User user = DbContext.Users.Where(x => x.UserGUID == UserGuid).FirstOrDefault();

                if (user == null)
                    return NotFound();

                DbContext.Users.Remove(user);

                await DbContext.SaveChangesAsync();

                LogCustom("The user has been deleted successfully.", name);
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
        /// Creates a user friend. You can only create your own user friends.
        /// </summary>
        /// <param name="UserGuid">User GUID (required)</param>
        /// <param name="request">Request</param>
        /// <returns>A response with the user friend you created</returns>
        /// <response code="201">If user friend was created successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you tried to add a friend to another user</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{UserGuid}/Friends/")]
        [ProducesResponseType(typeof(SingleResponse<UserFriendDTO>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostUserFriendAsync([BindRequired] Guid UserGuid, [FromBody] PostUserFriendRequest request)
        {
            string name = nameof(PostUserAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<UserFriendDTO>();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }


                var existingEntity = await DbContext
                    .GetUserFriendAsync(UserGuid, request.FriendGUID);

                if (existingEntity != null)
                    ModelState.AddModelError("FriendGuid", "This user is already your friend.");

                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(UserGuid);

                // Add entity to repository
                DbContext.UserFriends.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDTO();

                LogCustom("The user friend has been created successfully.", name);
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
        /// Updates an existing user friend. You can only update your own user friends.
        /// </summary>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <param name="FriendGuid">Friend Guid (required)</param>
        /// <param name="request">Request model</param>
        /// <returns>A response as update user result</returns>
        /// <response code="200">If user was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you try to edit another user's friends.</response>
        /// <response code="404">If the user or the user friend is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{UserGuid}/Friends/{FriendGuid}")]
        [ProducesResponseType(typeof(SingleResponse<UserFriendDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutUserFriendAsync([BindRequired] Guid UserGuid, [BindRequired] Guid FriendGuid, [FromBody] PutUserFriendRequest request)
        {
            string name = nameof(PutUserFriendAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<UserFriendDTO>();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetUserFriendAsync(UserGuid, FriendGuid);

                if (entity == null)
                    return NotFound();

                entity.IsBlocked = request.IsBlocked;

                DbContext.Update(entity);

                await DbContext.SaveChangesAsync();

                response.Model = entity.ReturnDTO();

                LogCustom("The user friend has been updated successfully.", name);
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
        /// Deletes an existing user friend. You can only delete your own user friends.
        /// </summary>
        /// <param name="UserGuid">User GUID (required)</param>
        /// <param name="FriendGuid">Friend GUID (required)</param>
        /// <returns>A successful delete response</returns>
        /// <response code="200">If the user friend was deleted successfully</response>
        /// <response code="400">For a bad request<</response>
        /// <response code="403">If you try to delete another user's friends</response>
        /// <response code="404">If the user or the user friend is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{UserGuid}/Friends/{FriendGuid}/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUserFriendAsync([BindRequired] Guid UserGuid, [BindRequired] Guid FriendGuid)
        {
            string name = nameof(DeleteUserFriendAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetUserFriendAsync(UserGuid, FriendGuid);

                if (entity == null)
                    return NotFound();

                DbContext.Remove(entity);

                await DbContext.SaveChangesAsync();

                LogCustom("The user friend has been deleted successfully.", name);
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
        /// Retrieves user meals. You can only retrieve your own meals.
        /// </summary>
        /// <param name="PageSize">Page size (optional)</param>
        /// <param name="PageNumber">Page number (optional)</param>
        /// <param name="UserGuid">User GUID (optional)</param>
        /// <param name="MealName">Meal Name (optional)</param>
        /// <param name="MealDescription">Meal Description (optional)</param>
        /// <param name="GlobalLink">Global Link Guid (optional)</param>
        /// <param name="MadeItBefore">Whether or not they have made it before (optional)</param>
        /// <returns>A response your meals that match the filters</returns>
        /// <response code="200">Returns the user meals list</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you try to acces another user's meals.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{UserGuid}/Meals")]
        [ProducesResponseType(typeof(PagedResponse<UserMealDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserMealsAsync([BindRequired] Guid UserGuid, int PageSize = 10, int PageNumber = 1,
            string MealName = null, string MealDescription = null, Guid? GlobalLink = null, bool? MadeItBefore = null)
        {

            string name = nameof(GetUserMealsAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new PagedResponse<UserMealDTO>();

            try
            {

                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var query = DbContext.GetUserMeals(UserGuid, MealName, MealDescription, GlobalLink, MadeItBefore);

                response.detailed = false;

                response.PageSize = PageSize;
                response.PageNumber = PageNumber;

                response.ItemsCount = await query.CountAsync();

                response.Model = (await query.Paging(PageSize, PageNumber).ToListAsync()).ConvertAll(x => x.ReturnDTO());

                response.Message = string.Format("Page {0} of {1}, Total of meals: {2}.", PageNumber, response.PageCount, response.ItemsCount);

                LogCustom("The user meals have been retrieved successfully.", name);
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
        /// Retrieves a meal by MealID. You can only retrieve your own meals.
        /// </summary>
        /// <param name="UserGuid">UserGuid (required)</param>
        /// <param name="MealID">Meal ID (required)</param>
        /// <returns>A response with a meal</returns>
        /// <response code="200">Returns the meal</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you try to retrieve a meal which is not yours</response>
        /// <response code="404">If the user or the meal does not exist</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{UserGuid}/Meals/{MealID}")]
        [ProducesResponseType(typeof(SingleResponse<UserMealDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserMealAsync(Guid UserGuid, int MealID)
        {

            string name = nameof(GetUserMealAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<UserMealDTO>();

            try
            {
                LogMethodInvoked(name);


                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                UserMeal meal = (await DbContext.GetUserMealByIDEditableAsync(UserGuid, MealID));

                if (meal != null)
                    response.Model = meal.ReturnDTO();

                response.detailed = false;

                LogCustom("The user meal has been retrieved successfully.", name);
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
        /// Creates a new meal. You can only create your own meals. Global Link and Private notes are optional.
        /// </summary>
        /// <param name="UserGuid">Cook Guid (Required)</param>
        /// <param name="request">Request model</param>
        /// <returns>A response with your new meal</returns>
        /// <response code="201">Returns the meal</response>
        /// <response code="400">For bad request</response>
        /// <response code="403">If you try to create a meal on behalf of another user, or you already have a meal named that.</response>
        /// <response code="404">If the user was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("{UserGuid}/Meals")]
        [ProducesResponseType(typeof(SingleResponse<UserMealDTO>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostUserMealAsync([BindRequired] Guid UserGuid, [FromBody] PostUserMealRequest request)
        {

            string name = nameof(PostUserMealAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<UserMealDTO>();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var existingEntity = await DbContext
                    .GetUserMealByMealNameAsync(UserGuid, request.mealName);

                if (existingEntity != null)
                {
                    ModelState.AddModelError("MealName", "Meal Name is already taken");
                    return Forbid();
                }


                if (!ModelState.IsValid)
                    return BadRequest();

                // Create entity from request model
                var entity = request.ToEntity(UserGuid);


                DbContext.UserMeals.Add(entity);

                // Save entity in database
                await DbContext.SaveChangesAsync();

                // Set the entity to response model
                response.Model = entity.ReturnDTO();
                response.detailed = false;

                LogCustom("The user meal has been created successfully.", name);
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
        /// Deletes an existing meal. You can only delete your own meals.
        /// </summary>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <param name="MealID">Meal ID (required)</param>
        /// <returns>A successful delete response</returns>
        /// <response code="200">If the meal was deleted successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you try to delete another user's meals.</response>
        /// <response code="404">If the user or the meal were not found.</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{UserGuid}/Meals/{MealID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUserMealAsync([BindRequired] Guid UserGuid, [BindRequired] int MealID)
        {
            string name = nameof(DeleteUserMealAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new Response();

            try
            {
                LogMethodInvoked(name);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                var entity = await DbContext.GetUserMealByIDEditableAsync(UserGuid, MealID);

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

                if (entity.HostGuid != UserGuid)
                    return Forbid();

                // Remove entity from repository
                DbContext.Remove(entity);

                // Delete entity in database
                await DbContext.SaveChangesAsync();

                LogCustom("The user meal has been created successfully.", name);
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
        /// Updates an existing meal. You can only update your own meals.
        /// </summary>
        /// <param name="MealID">Meal ID (required)</param>
        /// <param name="UserGuid">User Guid (required)</param>
        /// <param name="request">Request model</param>
        /// <returns>A successful response to updating your meal.</returns>
        /// <response code="200">If meal was updated successfully</response>
        /// <response code="400">For a bad request</response>
        /// <response code="403">If you try to update someone else's meal</response>
        /// <response code="404">If the user or the meal is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{UserGuid}/Meals/{MealID}")]
        [ProducesResponseType(typeof(SingleResponse<UserMealDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutUserMealsAsync([BindRequired] Guid UserGuid, [BindRequired] int MealID, [FromBody] PutUserMealRequest request)
        {
            string name = nameof(PutUserMealsAsync);

            UserIdentity id = APIServices.GetUserID(HttpContext.User.Claims);

            var response = new SingleResponse<UserMealDTO>();

            try
            {
                LogMethodInvoked(name);

                // Get stock item by id
                var entity = await DbContext.GetUserMealByIDEditableAsync(UserGuid, MealID);

                if (!(UserGuid != id.UserGuid))
                {
                    LogGatekeeperInfraction_NotSameUser(id.AppInstallGuid, id.UserGuid, name);
                    return Forbid();
                }

                // Validate if entity exists
                if (entity == null)
                    return NotFound();

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

                response.Model = entity.ReturnDTO();
                response.detailed = false;

                LogCustom("The user meal has been updated successfully.", name);
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
