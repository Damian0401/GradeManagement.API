using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Message
{
    public class SendMessageDtoRequest
    {
        public string RecevierId { get; set; }
        public string Title { get; set; }
        public string Payload { get; set; }
        public DateTime PostedOn { get; set; }
    }
}
