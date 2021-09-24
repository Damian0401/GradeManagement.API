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

        [Produces(typeof(ServiceResponse<GetMessageByIdDtoResponse>))]
        [Authorize]
        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetById(Guid messageId)
        {
            var response = await _messageService.GetMessageByIdAsync(messageId);

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> Send(SendMessageDtoRequest dto)
        {
            var response = await _messageService.SendMessageAsync(dto);

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetSentMessagesDtoResponse>))]
        [Authorize]
        [HttpGet("sent")]
        public async Task<IActionResult> Sent()
        {
            var response = await _messageService.GetSentMessagesAsync();

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetReceivedMessagesDtoResponse>))]
        [Authorize]
        [HttpGet("received")]
        public async Task<IActionResult> Received()
        {
            var response = await _messageService.GetReceivedMessagesAsync();

            return SendResponse(response);
        }
    }
}
