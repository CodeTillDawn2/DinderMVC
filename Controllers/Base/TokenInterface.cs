using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public interface TokenInterface
    {

        public abstract Task<IActionResult> GetTokenAsync(Guid appInstallID);


#pragma warning restore CS1591
    }
}
