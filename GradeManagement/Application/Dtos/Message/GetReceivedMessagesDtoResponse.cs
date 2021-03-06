using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Message
{
    public class GetReceivedMessagesDtoResponse
    {
        public List<MessageForGetReceivedMessagesDtoResponse> Messages { get; set; }
    }

    public class MessageForGetReceivedMessagesDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PostedOn { get; set; }
        public bool IsRead { get; set; }
    }
}
