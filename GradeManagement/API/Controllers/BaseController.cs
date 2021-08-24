using Application.Services.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult SendResponse(ServiceResponse response)
        {
            return response.ResponseType switch
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
            return response.ResponseType switch
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
