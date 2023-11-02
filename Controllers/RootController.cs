using DataModels;
using DinderBackEndv2.Models;
using DinderBackEndv2.Queries;
using DinderBackEndv2.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DinderBackEndv2.Controllers
{
#pragma warning disable CS1591

    [ApiController]
    [Route("api/v1/[controller]")]
    public class RootController : DinderControllerBase<RootController>, RootInterface
    {







        public RootController(ILogger<RootController> logger, DinderContext dbContext) : base(logger, dbContext)
        {

        }

#pragma warning restore CS1591


        // GET
        // api/v1/Roots/

        /// <summary>
        /// Retrieves roots
        /// </summary>
        /// <param name="appInstallID">AppInstallID</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>A response with stock items list</returns>
        /// <response code="200">Returns the stock items list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRootsAsync(Guid appInstallID, int pageSize = 100, int pageNumber = 1)
        {
            string name = nameof(GetRootsAsync);
            LogMethodInvoked(name);

            var response = new PagedResponse<RootDM>();

            try
            {

                if (!(await DbContext.AppInstallRegistered(appInstallID)))
                {
                    LogInvalidInstall(appInstallID, name);
                    return BadRequest();
                }


                // Get the "proposed" query from repository
                var query = DbContext.GetRoots();


                // Set paging values
                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                // Get the total rows
                response.ItemsCount = query.Count();

                // Get the specific page from database
                response.Model = query.Paging(pageSize, pageNumber).ToList();

                LogCustom("The roots have been retrieved successfully.", name);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact technical support.";

                LogError(ex, name);
            }

            return response.ToHttpResponse();
        }



    }
}
