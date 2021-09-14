using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class EditUserProfileDtoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string ImageUrl { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
