using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Payload { get; set; }
        public DateTime PostedOn { get; set; }
        public bool IsRead { get; set; }
        public string UserFromId { get; set; }
        public virtual ApplicationUser UserFrom { get; set; }
        public string UserToId { get; set; }
        public virtual ApplicationUser UserTo { get; set; }
    }
}
