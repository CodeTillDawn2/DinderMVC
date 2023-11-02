using DataModels;
using DinderBackEndv2.Models;
using DinderBackEndv2.Queries;
using DinderBackEndv2.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Requests;
using System;
using System.Threading.Tasks;

namespace DinderBackEndv2.Controllers
{
#pragma warning disable CS1591

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppInstallsController : DinderControllerBase<AppInstallsController>, AppInstallsInterface
    {


        public AppInstallsController(ILogger<AppInstallsController> logger, DinderContext dbContext) : base(logger, dbContext)
        {
        }

#pragma warning restore CS1591


        // GET
        // api/v1/AppInstalls/{LookupGUID}

        /// <summary>
        /// Retrieves a App Install by GUID
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with an App Install</returns>
        /// <response code="200">Returns the App Install list</response>
        /// <response code="404">If App Install is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [NonAction] // Turned off for now
        [HttpGet("{LookupGUID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAppInstallAsync([FromBody] GetAppInstallRequest request)
        {
            string name = nameof(GetAppInstallAsync);

            LogMethodInvoked(name);

            if (!(await DbContext.AppInstallRegistered(request.appInstallID)))
            {
                LogInvalidInstall(request.appInstallID, name);
                return BadRequest();
            }

            //AppInstallDM AI = DbContext.AppInstalls.Where(x => x.AppInstallGUID == request.appInstallID).Select(x => x.ReturnDTO()).FirstOrDefault();
            //if (AI == null)
            //{
            //    ModelState.AddModelError("AppInstall", "Your app install cannot be validated. Try reinstalling.");
            //    AppInstallLogger?.LogDebug("'{0}' has had an invalid install validation attempt using " + request.appInstallID.ToString(), nameof(GetAppInstallAsync));
            //    return BadRequest();
            //}

            var response = new SingleResponse<AppInstallDM>();

            try
            {
                // Get the Guid by id
                response.Model = await DbContext.VerifyInstall(new AppInstall(request.lookupGUID));
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }

        //    // POST
        //    // api/v1/AppInstall/

        //    /// <summary>
        //    /// Creates a new App Install
        //    /// </summary>
        //    /// <param name="request">Request model</param>
        //    /// <returns>A response with new App Install</returns>
        //    /// <response code="200">Returns the App Install list</response>
        //    /// <response code="201">A response as creation of App Install</response>
        //    /// <response code="400">For bad request</response>
        //    /// <response code="500">If there was an internal server error</response>
        //    [NonAction] // Turned off for now
        //    [HttpPost("{appInstall}")]
        //    [ProducesResponseType(200)]
        //    [ProducesResponseType(201)]
        //    [ProducesResponseType(400)]
        //    [ProducesResponseType(500)]
        //    public async Task<IActionResult> PostAppInstallAsync([FromBody] PostAppInstallRequest request)
        //    {
        //        AppInstallLogger?.LogDebug("'{0}' has been invoked", nameof(PostAppInstallAsync));

        //        var response = new SingleResponse<AppInstallDM>();

        //        try
        //        {
        //            //var existingEntity = await DbContext
        //            //    .VerifyInstall(new AppInstall
        //            //    {
        //            //       IPAddress  = request.IPAddress
        //            //    });

        //            //if (existingEntity != null)
        //            //    ModelState.AddModelError("UserName", "Username is already taken");

        //            if (!ModelState.IsValid)
        //                return BadRequest();

        //            // Create entity from request model
        //            var entity = request.ToEntity();

        //            // Add entity to repository
        //            DbContext.AppInstalls.Add(entity);

        //            // Save entity in database
        //            await DbContext.SaveChangesAsync();

        //            // Set the entity to response model
        //            response.Model = entity.ReturnDTO();
        //        }
        //        catch (Exception ex)
        //        {
        //            response.DidError = true;
        //            response.ErrorMessage = "There was an internal error, please contact to technical support.";

        //            AppInstallLogger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(PostAppInstallAsync), ex);
        //        }

        //        return response.ToHttpResponse();
        //    }

    }
}
