using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Notes
{
    public class GetMyNotesDtoResponse
    {
        public List<NoteForGetMyNotesDtoResponse> Notes { get; set; }
    }

    public class NoteForGetMyNotesDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Payload { get; set; }
    }
}
