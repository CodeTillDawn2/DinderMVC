using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface RootInterface
    {

        public abstract Task<IActionResult> GetVersions();
        public abstract Task<IActionResult> GetVersion1Endpoints();


#pragma warning restore CS1591
    }
}
