using Application.Services.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult SendResponse(ServiceResponse response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.OK => Ok(),
                HttpStatusCode.Unauthorized => Unauthorized(),
                HttpStatusCode.Forbidden => Forbid(),
                HttpStatusCode.NotFound => NotFound(),
                _ => BadRequest(response.Message),
            };
        }

        protected IActionResult SendResponse<T>(ServiceResponse<T> response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.OK => Ok(response.ResponseContent),
                HttpStatusCode.Unauthorized => Unauthorized(),
                HttpStatusCode.Forbidden => Forbid(),
                HttpStatusCode.NotFound => NotFound(),
                _ => BadRequest(response.Message),
            };
        }
    }
}
