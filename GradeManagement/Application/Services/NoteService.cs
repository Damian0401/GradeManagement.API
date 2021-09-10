using Application.Dtos.Note;
using Application.Dtos.Notes;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NoteService : Service, INoteService
    {
        private readonly ILogger _logger;
        public NoteService(ILogger<NoteService> logger, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _logger = logger;
        }

        public async Task<ServiceResponse<CreateNoteDtoResponse>> CreateNoteAsync(CreateNoteDtoRequest dto)
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.Unauthorized);

            var note = Mapper.Map<Note>(dto);

            note.UserId = CurrentlyLoggedUser.Id;

            Context.Notes.Add(note);

            var result = await Context.SaveChangesAsync();

            var responseDto = Mapper.Map<CreateNoteDtoResponse>(note);

            return result > 0
                ? new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.BadRequest, "Unable to save note in datebase");
        }

        public async Task<ServiceResponse> DeleteNoteAsync(Guid noteId)
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var note = await Context.Notes.FirstOrDefaultAsync(x => x.Id.Equals(noteId));

            if (note is null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (CurrentlyLoggedUser.Role != Role.Administrator
                && CurrentlyLoggedUser.Id != note.UserId)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            Context.Notes.Remove(note);

            var result = await Context.SaveChangesAsync();

            return result > 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Unable to delete note");
        }

        public async Task<ServiceResponse<GetAllNotesDtoResponse>> GetAllNotesAsync()
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.Forbidden);

            var notes = await Context.Notes.ToListAsync();

            var responseDto = new GetAllNotesDtoResponse { Notes = Mapper.Map<List<NoteForGetAllNotesDtoResponse>>(notes) };

            return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.OK, responseDto);
        }
    }
}
