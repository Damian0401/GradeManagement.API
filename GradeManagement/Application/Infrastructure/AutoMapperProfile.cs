﻿using Application.Dtos.User;
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
        }

        private void MapsForUser()
        {
            CreateMap<RegisterUserDtoRequest, ApplicationUser>();
            CreateMap<ApplicationUser, RegisterUserDtoResponse>();
        }
    }
}