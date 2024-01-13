using DinderDLL.Models;
using DinderMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DinderMVC.Controllers
{
#pragma warning disable CS1591
    public class DinderControllerBase<T> : ControllerBase
    {
        private readonly ILogger Logger;
        protected readonly DinderContext DbContext;


        public DinderControllerBase(ILogger<T> logger, DinderContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [NonAction]
        public void LogMethodInvoked(string NameOfMethod)
        {
            Logger?.LogDebug("'{0}' has been invoked", NameOfMethod);
        }
        [NonAction]
        public void LogInvalidInstall(Guid AppInstallID, string NameOfMethod)
        {
            Logger?.LogDebug("'{0}' has had an invalid install validation attempt using " + AppInstallID.ToString(), NameOfMethod);
        }
        [NonAction]
        public void LogGatekeeperInfraction(Guid AppInstallID, Guid UserGuid, string NameOfMethod)
        {
            Logger?.LogDebug("'{1}' has had a user, {2} attempting to see a party they are not authorized to see using " + AppInstallID.ToString(), UserGuid, NameOfMethod);
        }
        [NonAction]
        public void LogCustom(string Message, string NameOfMethod)
        {
            Logger?.LogInformation(Message, NameOfMethod);
        }
        [NonAction]
        public void LogError(Exception ex, string NameOfMethod)
        {
            Logger?.LogCritical("There was an error on '{0}' invocation: {1}", NameOfMethod, ex);
        }
        [NonAction]
        public void LogInvalidUser(Guid UserGuid, string NameOfMethod)
        {
            Logger?.LogDebug("'{0}' has had an invalid user guid using " + UserGuid.ToString(), NameOfMethod);
        }


#pragma warning restore CS1591
    }
}
