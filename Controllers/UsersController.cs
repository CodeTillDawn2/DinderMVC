using DataModels;
using DinderBackEndv2.Models;
using DinderBackEndv2.Queries;
using DinderBackEndv2.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Requests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DinderBackEndv2.Controllers
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
        /// Retrieves users
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

                response.Model = await DbContext.GetUsersAsync(pageSize, pageNumber, displayName);

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
        /// Retrieves user friends
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

                response.Model = await DbContext.GetUserFriendsAsync(userGuid);
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
        /// Retrieves user Parties
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



                response.Model = await DbContext.GetUserPartiesAsync(userGuid);
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
        /// Retrieves user meals
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="userGuid">User Guid</param>
        /// <returns>A response with stock items list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{userGuid}/Meals")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserMealsAsync(Guid appInstallID, Guid userGuid, int pageSize = 10, int pageNumber = 1)
        {

            string name = nameof(GetUserMealsAsync);

            var response = new PagedResponse<MealDM>();
            try
            {
                LogMethodInvoked(name);

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }

                response.Model = await DbContext.GetUserMealsAsync(userGuid);

                response.ItemsCount = response.Model.Count();
                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

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
        /// Retrieves a user by GUID
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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // POST
        // api/v1/Users/

        /// <summary>
        /// Creates a new user
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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // PUT
        // api/v1/Users/5

        /// <summary>
        /// Updates an existing user
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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // DELETE
        // api/v1/Users/Users/5

        /// <summary>
        /// Deletes an existing user
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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // POST
        // api/v1/Users/{UserGUID}/Friends

        /// <summary>
        /// Creates a user friend
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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // PUT
        // api/v1/Users/5

        /// <summary>
        /// Updates an existing user friend
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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        // Delete
        // api/v1/Users/{UserGUID}/Friends

        /// <summary>
        /// Deletes an existing user friend
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
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

    }
}
