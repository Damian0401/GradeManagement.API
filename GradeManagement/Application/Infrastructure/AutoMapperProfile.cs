using Application.Dtos.Message;
using Application.Dtos.Note;
using Application.Dtos.Notes;
using Application.Dtos.User;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForUser();
            MapsForNotes();
            MapsForMessages();
        }

        private void MapsForUser()
        {
            CreateMap<RegisterUserDtoRequest, ApplicationUser>();
            CreateMap<ApplicationUser, RegisterUserDtoResponse>();
            CreateMap<LoginUserDtoRequest, ApplicationUser>();
            CreateMap<ApplicationUser, LoginUserDtoResponse>();
            CreateMap<ApplicationUser, UserForGetAllUsersDtoResponse>();
            CreateMap<ApplicationUser, StudentForGetAllStudentsDtoResponse>();
            CreateMap<ApplicationUser, GetUserByIdDtoResponse>();
            CreateMap<ApplicationUser, TeacherForGetAllStudentsDtoResponse>();
            CreateMap<EditUserProfileDtoRequest, ApplicationUser>();
            CreateMap<ApplicationUser, EditUserProfileDtoResponse>();
        }

        private void MapsForNotes()
        {
            CreateMap<Note, NoteForGetAllNotesDtoResponse>();
            CreateMap<CreateNoteDtoRequest, Note>();
            CreateMap<Note, CreateNoteDtoResponse>();
            CreateMap<Note, NoteForGetMyNotesDtoResponse>();
            CreateMap<UpdateNoteDtoRequest, Note>();
            CreateMap<Note, UpdateNoteDtoResponse>();
            CreateMap<Note, GetNoteByIdDtoResponse>();
        }

        private void MapsForMessages()
        {
            CreateMap<SendMessageDtoRequest, Message>();
        }
    }
}
