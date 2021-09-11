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
        Task<ServiceResponse<LoginUserDtoResponse>> LoginUserAsync(LoginUserDtoRequest dto);
        Task<ServiceResponse<GetAllUsersDtoResponse>> GetAllUsersAsync();
        Task<ServiceResponse<GetAllStudentsDtoResponse>> GetAllStudentsAsync();
        Task<ServiceResponse> DeleteUserAsync(string userId);
        Task<ServiceResponse<GetUserByIdDtoResponse>> GetUserByIdAsync(string userId);
    }
}
