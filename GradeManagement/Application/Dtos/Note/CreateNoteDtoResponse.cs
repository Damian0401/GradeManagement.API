using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Notes
{
    public class CreateNoteDtoResponse
    {
        public string Title { get; set; }

        public string Payload { get; set; }

        public Guid Id { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
