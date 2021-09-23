using Application.Dtos.Message;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MessageService : Service, IMessageService
    {
        public MessageService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<ServiceResponse<GetReceivedMessagesDtoResponse>> GetReceivedMessagesAsync()
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<GetReceivedMessagesDtoResponse>(HttpStatusCode.Unauthorized);

            var messages = await Context.Messages.Where(x => x.UserToId.Equals(CurrentlyLoggedUser.Id)).ToListAsync();

            var responseDto = new GetReceivedMessagesDtoResponse { Messages = Mapper.Map<List<MessageForGetReceivedMessagesDtoResponse>>(messages) };

            return new ServiceResponse<GetReceivedMessagesDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse<GetSentMessagesDtoResponse>> GetSentMessagesAsync()
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<GetSentMessagesDtoResponse>(HttpStatusCode.Unauthorized);

            var messages = await Context.Messages.Where(x => x.UserFromId.Equals(CurrentlyLoggedUser.Id)).ToListAsync(); ;

            var responseDto = new GetSentMessagesDtoResponse { Messages = Mapper.Map<List<MessageForGetSentMessagesDtoResponse>>(messages) };

            return new ServiceResponse<GetSentMessagesDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse> SendMessageAsync(SendMessageDtoRequest dto)
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Id.Equals(dto.ReceiverId))
                return new ServiceResponse(HttpStatusCode.BadRequest, "You can not send message to yourself");

            var userTo = await Context.Users.FirstOrDefaultAsync(x => x.Id.Equals(dto.ReceiverId));

            if (userTo is null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var message = Mapper.Map<Message>(dto);

            message.UserToId = dto.ReceiverId;

            message.UserFromId = CurrentlyLoggedUser.Id;

            Context.Messages.Add(message);

            var result = await Context.SaveChangesAsync();

            return result > 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Unalble to send message");
        }
    }
}
