using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DinderBackEndv2.Controllers
{
#pragma warning disable CS1591
    public interface RootInterface
    {

        public abstract Task<IActionResult> GetRootsAsync(Guid appInstallID, int pageSize = 100, int pageNumber = 1);


#pragma warning restore CS1591
    }
}
