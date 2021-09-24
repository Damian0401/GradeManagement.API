using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Message
{
    public class GetSentMessagesDtoResponse
    {
        public List<MessageForGetSentMessagesDtoResponse> Messages { get; set; }
    }

    public class MessageForGetSentMessagesDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PostedOn { get; set; }
        public bool IsRead { get; set; }
    }
}
