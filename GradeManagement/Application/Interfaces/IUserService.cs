using Application.Dtos.User;
using Application.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<RegisterUserDtoResponse>> RegisterUserAsync(RegisterUserDtoRequest dto);
    }
}
