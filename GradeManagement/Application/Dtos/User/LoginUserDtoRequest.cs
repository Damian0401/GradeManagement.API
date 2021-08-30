using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class LoginUserDtoRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
