using Application.Dtos.Message;
using Application.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMessageService
    {
        Task<ServiceResponse> SendMessageAsync(SendMessageDtoRequest dto);
        Task<ServiceResponse<GetSentMessagesDtoResponse>> GetSentMessagesAsync();
        Task<ServiceResponse<GetReceivedMessagesDtoResponse>> GetReceivedMessagesAsync();
        Task<ServiceResponse<GetMessageByIdDtoResponse>> GetMessageByIdAsync(Guid messageId);
    }
}
