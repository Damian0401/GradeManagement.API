using Application.Dtos.Note;
using Application.Dtos.Notes;
using Application.Interfaces;
using Application.Services.Utilities;
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
        public async Task<IActionResult> GetNotes()
        {
            var response = await _noteService.GetAllNotesAsync();

            return SendResponse(response);
        }

        [Produces(typeof(GetNoteByIdDtoResponse))]
        [Authorize]
        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetById(Guid noteId)
        {
            var response = await _noteService.GetNoteByIdAsync(noteId);

            return SendResponse(response);
        }

        [Produces(typeof(GetMyNotesDtoResponse))]
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotes()
        {
            var response = await _noteService.GetMyNotesAsync();

            return SendResponse(response);
        }

        [Produces(typeof(CreateNoteDtoResponse))]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateNoteDtoRequest dto)
        {
            var response = await _noteService.CreateNoteAsync(dto);

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize]
        [HttpDelete("{noteId}")]
        public async Task<IActionResult> Delete([FromRoute]Guid noteId)
        {
            var response = await _noteService.DeleteNoteAsync(noteId);

            return SendResponse(response);
        }

        [Produces(typeof(UpdateNoteDtoResponse))]
        [Authorize]
        [HttpPut("{noteId}")]
        public async Task<IActionResult> Update(Guid noteId, UpdateNoteDtoRequest dto)
        {
            var response = await _noteService.UpdateNoteAsync(noteId, dto);

            return SendResponse(response);
        }
    }
}
