using DinderDLL.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface GlobalMealsInterface
    {

        public abstract Task<IActionResult> GetGlobalMealsAsync(Guid appInstallID, int pageSize = 10, int pageNumber = 1,
            string mealName = null, string mealDescription = null);
        public abstract Task<IActionResult> GetGlobalMealAsync(Guid appInstallID, Guid globalMealGuid);
        public abstract Task<IActionResult> PostGlobalMealAsync([FromBody] PostGlobalMealRequest request);
        public abstract Task<IActionResult> PutGlobalMealAsync(Guid userGuid, int MealID, [FromBody] PutGlobalMealRequest request);

#pragma warning restore CS1591
    }
}
