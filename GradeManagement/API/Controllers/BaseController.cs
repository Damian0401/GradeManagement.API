using Application.Services.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult SendResponse(ServiceResponse response)
        {
            switch (response.ResponseType)
            {
                case HttpStatusCode.OK:
                    return Ok();
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case HttpStatusCode.Forbidden:
                    return Forbid();
                case HttpStatusCode.NotFound:
                    return NotFound();
                default:
                    return BadRequest(response.Message);
            }
        }

        protected IActionResult SendResponse<T>(ServiceResponse<T> response)
        {
            switch (response.ResponseType)
            {
                case HttpStatusCode.OK:
                    return Ok(response.ResponseContent);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case HttpStatusCode.Forbidden:
                    return Forbid();
                case HttpStatusCode.NotFound:
                    return NotFound();
                default:
                    return BadRequest(response.Message);
            }
        }
    }
}
