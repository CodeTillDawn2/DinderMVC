using DinderDLL.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface UsersInterface
    {

        public abstract Task<IActionResult> GetUsersAsync(int pageSize = 10, int pageNumber = 1, string displayName = null);


        public abstract Task<IActionResult> GetUserPartiesAsync(Guid userGuid);

        public abstract Task<IActionResult> GetUserAsync(Guid UserGuid);

        public abstract Task<IActionResult> PostUserAsync([FromBody] PostUserRequest request);

        public abstract Task<IActionResult> PutUsersAsync(Guid userGuid, [FromBody] PutUserRequest request);

        public abstract Task<IActionResult> DeleteUserAsync(Guid userGuid);

        public abstract Task<IActionResult> GetUserFriendsAsync(Guid userGuid);

        public abstract Task<IActionResult> PostUserFriendAsync(Guid userGuid, [FromBody] PostUserFriendRequest request);
        public abstract Task<IActionResult> PutUserFriendAsync(Guid userGuid, Guid friendGUID, [FromBody] PutUserFriendRequest request);
        public abstract Task<IActionResult> DeleteUserFriendAsync(Guid userGuid, Guid friendGuid);
        public abstract Task<IActionResult> GetUserMealsAsync(Guid userGUID, int pageSize = 10, int pageNumber = 1,
            int? mealID = null, string mealName = null, string mealDescription = null, Guid? globalLink = null, bool? madeItBefore = null);

        public abstract Task<IActionResult> GetUserMealAsync(Guid UserGuid, int mealID);

        public abstract Task<IActionResult> PostUserMealAsync(Guid UserGuid, [FromBody] PostUserMealRequest request);

        public abstract Task<IActionResult> PutUserMealsAsync(Guid userGuid, int MealID, [FromBody] PutUserMealRequest request);

        public abstract Task<IActionResult> DeleteUserMealAsync(Guid userGuid, int mealID);
#pragma warning restore CS1591
    }
}
