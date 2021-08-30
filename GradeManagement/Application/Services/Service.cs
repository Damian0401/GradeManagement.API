using Presistence;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Domain.Models;
using AutoMapper;
using Application.Interfaces;

namespace Application.Services
{
    public class Service
    {
        protected DataBaseContext Context { get; }
        protected UserManager<ApplicationUser> UserManager { get; }
        protected IMapper Mapper { get; }
        protected string CurrentlyLoggedUserName { get; }
        protected ApplicationUser CurrentlyLoggedUser { get; set; }

        public Service(IServiceProvider serviceProvider)
        {
            Context = serviceProvider.GetService<DataBaseContext>();

            UserManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            Mapper = serviceProvider.GetService<IMapper>();

            var userAccesor = serviceProvider.GetService<IUserAccessor>();

            CurrentlyLoggedUserName = userAccesor.GetCurrentlyLoggedUserName();

            AssignCurrentlyLoggedUser();
        }

        private void AssignCurrentlyLoggedUser()
        {
            if (CurrentlyLoggedUserName is null)
            {
                CurrentlyLoggedUser = null;
                return;
            }

            CurrentlyLoggedUser = UserManager.FindByNameAsync(CurrentlyLoggedUserName).Result;
        }
    }
}
