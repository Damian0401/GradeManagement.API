using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Notes
{
    public class GetNoteByIdDtoResponse
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Payload { get; set; }
    }
}
