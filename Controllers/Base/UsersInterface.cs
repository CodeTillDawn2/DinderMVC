using Microsoft.AspNetCore.Mvc;
using Requests;
using System;
using System.Threading.Tasks;

namespace DinderBackEndv2.Controllers
{
#pragma warning disable CS1591
    public interface UsersInterface
    {

        public abstract Task<IActionResult> GetUsersAsync(Guid appInstallID, int pageSize = 10, int pageNumber = 1, string displayName = null, DateTime? createDate = null, DateTime? lastActiveDate = null);


        public abstract Task<IActionResult> GetUserPartiesAsync(Guid appInstallID, Guid userGuid);

        public abstract Task<IActionResult> GetUserMealsAsync(Guid appInstallID, Guid userGuid, int pageSize = 10, int pageNumber = 1);

        public abstract Task<IActionResult> GetUserAsync(Guid appInstallID, Guid UserGuid);

        public abstract Task<IActionResult> PostUserAsync([FromBody] PostUserRequest request);

        public abstract Task<IActionResult> PutUsersAsync(Guid userGuid, [FromBody] PutUserRequest request);

        public abstract Task<IActionResult> DeleteUserAsync(Guid appInstallID, Guid userGuid);

        public abstract Task<IActionResult> GetUserFriendsAsync(Guid appInstallID, Guid userGuid);

        public abstract Task<IActionResult> PostUserFriendAsync(Guid userGuid, [FromBody] PostUserFriendRequest request);
        public abstract Task<IActionResult> PutUserFriendAsync(Guid userGuid, Guid friendGUID, [FromBody] PutUserFriendRequest request);
        public abstract Task<IActionResult> DeleteUserFriendAsync(Guid appInstallID, Guid userGuid, Guid friendGuid);

#pragma warning restore CS1591
    }
}
