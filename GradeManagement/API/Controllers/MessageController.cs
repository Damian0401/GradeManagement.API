using Application.Dtos.Message;
using Application.Interfaces;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> Send(SendMessageDtoRequest dto)
        {
            var response = await _messageService.SendMessageAsync(dto);

            return SendResponse(response);
        }
    }
}
