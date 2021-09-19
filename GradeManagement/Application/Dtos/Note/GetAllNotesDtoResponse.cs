using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Note
{
    public class GetAllNotesDtoResponse
    {
        public List<NoteForGetAllNotesDtoResponse> Notes { get; set; }
    }

    public class NoteForGetAllNotesDtoResponse
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Payload { get; set; }
    }
}
