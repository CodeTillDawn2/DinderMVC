using DinderDLL.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface AppInstallsInterface
    {

        public abstract Task<IActionResult> GetAppInstallAsync([FromBody] GetAppInstallRequest request);


#pragma warning restore CS1591
    }
}
