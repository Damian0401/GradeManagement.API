using API.Security;
using Application.Dtos.User;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Produces(typeof(ServiceResponse<RegisterUserDtoResponse>))]
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]RegisterUserDtoRequest dto)
        {
            var response = await _userService.RegisterUserAsync(dto);

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<LoginUserDtoResponse>))]
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDtoRequest dto)
        {
            var response = await _userService.LoginUserAsync(dto);  

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllUsersDtoResponse>))]
        [AuthorizeRoles(Role.Administrator)]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetAllUsersAsync();

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetUserByIdDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Teacher)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute]string userId)
        {
            var response = await _userService.GetUserByIdAsync(userId);

            return SendResponse(response);
        }

        [Authorize]
        [HttpGet("Students")]
        public async Task<IActionResult> GetStudents()
        {
            var response = await _userService.GetAllStudentsAsync();

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var response = await _userService.DeleteUserAsync(userId);

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllTeachersDtoResponse>))]
        [Authorize]
        [HttpGet("Teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var response = await _userService.GetAllTeachersAsync();

            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<EditUserProfileDtoResponse>))]
        [Authorize]
        [HttpPut("")]
        public async Task<IActionResult> EditProfile([FromForm]EditUserProfileDtoRequest dto)
        {
            var response = await _userService.EditUserProfileAsync(dto);

            return SendResponse(response);
        }
    }
}
