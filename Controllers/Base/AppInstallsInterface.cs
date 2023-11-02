using Microsoft.AspNetCore.Mvc;
using Requests;
using System.Threading.Tasks;

namespace DinderBackEndv2.Controllers
{
#pragma warning disable CS1591
    public interface AppInstallsInterface
    {

        public abstract Task<IActionResult> GetAppInstallAsync([FromBody] GetAppInstallRequest request);


#pragma warning restore CS1591
    }
}
