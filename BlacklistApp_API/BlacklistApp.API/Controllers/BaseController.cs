using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlacklistApp.API.Controllers
{
    public class BaseController : ControllerBase
    {

        /// <summary>
        /// Returns the response.
        /// </summary>
        /// <param name="data">The data.</param> 
        /// <returns></returns>
        protected IActionResult ReturnResponse(dynamic data)
        {
            if (data.IsSuccessful == true)
                return Ok(data);
            else if (data.StatusCode == "401")
            {
                return Unauthorized(data);
            }
            else if (data.StatusCode == "404")
            {
                return NotFound(data);
            }
            else if (data.StatusCode == "500")
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, data);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest, data);
            }
        }
        /// <summary> 
        /// Returns the response.
        /// </summary> 
        /// <param name="data">The data.</param> 
        /// <returns></returns> 
        protected IActionResult SendResponse<T>(Result<T> data)
        {
            if (data.Success)
                return StatusCode(data.Status, data);
            else if (data.Status == 401)
            {
                return Unauthorized(data);
            }
            else if (data.Status == 404)
            {
                return NotFound(data);
            }
            else if (data.Status == 500)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, data);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest, data);
            }
        }
    }
}
