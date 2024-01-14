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


#pragma warning restore CS1591
    }
}
