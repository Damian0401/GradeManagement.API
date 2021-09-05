using Application.Dtos.Note;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class NotesController : BaseController
    {
        private readonly INoteService _noteService;
        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [Produces(typeof(GetAllNotesDtoResponse))]
        [Authorize(Roles = Role.Administrator)]
        [HttpGet]
        public async Task<IActionResult> Sample()
        {
            var response = await _noteService.GetAllNotesAsync();

            return SendResponse(response);
        }
    }
}
