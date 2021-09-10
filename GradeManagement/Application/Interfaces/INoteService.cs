using Application.Dtos.Note;
using Application.Dtos.Notes;
using Application.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INoteService
    {
        Task<ServiceResponse<GetAllNotesDtoResponse>> GetAllNotesAsync();
        Task<ServiceResponse<CreateNoteDtoResponse>> CreateNoteAsync(CreateNoteDtoRequest dto);
        Task<ServiceResponse> DeleteNoteAsync(Guid noteId);
        Task<ServiceResponse<GetMyNotesDtoResponse>> GetMyNotesAsync();
        Task<ServiceResponse<UpdateNoteDtoResponse>> UpdateNoteAsync(Guid noteId, UpdateNoteDtoRequest dto);
        Task<ServiceResponse<GetNoteByIdDtoResponse>> GetNoteByIdAsync(Guid noteId);
    }
}
