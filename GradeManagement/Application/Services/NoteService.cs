using Application.Dtos.Note;
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
