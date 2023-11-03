using DinderDLL.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface MealsInterface
    {

        public abstract Task<IActionResult> GetMealsAsync(Guid appInstallID, int pageSize = 10, int pageNumber = 1, Guid? userGUID = null,
            int? mealID = null, string mealName = null, string mealDescription = null, Guid? globalLink = null, bool? madeItBefore = null);

        public abstract Task<IActionResult> GetMealAsync(Guid appInstallID, int mealID);

        public abstract Task<IActionResult> PostMealAsync([FromBody] PostMealRequest request);

        public abstract Task<IActionResult> PutMealsAsync(int MealID, [FromBody] PutMealRequest request);

        public abstract Task<IActionResult> DeleteMealAsync(Guid appInstallID, Guid userGuid, int mealID);
#pragma warning restore CS1591
    }
}
