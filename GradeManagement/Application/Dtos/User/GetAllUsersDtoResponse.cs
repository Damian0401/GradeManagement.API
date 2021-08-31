using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class GetAllUsersDtoResponse
    {
        public List<UserForGetAllUsersDtoResponse> Users { get; set; }
    }

    public class UserForGetAllUsersDtoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }
}
