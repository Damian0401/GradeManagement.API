using Application.Dtos.User;
using Application.Interfaces;
using Application.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Application.Services
{
    public class UserService : Service, IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IWebHostEnvironment _hostEnvironment;
        public UserService(IServiceProvider serviceProvider, ILogger<UserService> logger, 
            SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostEnvironment) 
            : base(serviceProvider)
        {
            _logger = logger;

            _signInManager = signInManager;

            _hostEnvironment = hostEnvironment;
        }

        public async Task<ServiceResponse<RegisterUserDtoResponse>> RegisterUserAsync(RegisterUserDtoRequest dto)
        {
            var validationResonse = await ValidateRegisterRequestAsync(dto);

            if (validationResonse.StatusCode != HttpStatusCode.OK)
                return validationResonse;

            var userToRegister = Mapper.Map<ApplicationUser>(dto);

            if (dto.Role.Equals(Role.Administrator) || dto.Role.Equals(Role.Teacher))
                return await RegisterAdminOrTeacherAsync(dto, userToRegister);

            return await RegisterStudentAsync(dto, userToRegister);
        }

        private async Task<ServiceResponse<RegisterUserDtoResponse>> ValidateRegisterRequestAsync(RegisterUserDtoRequest dto)
        {
            if (await Context.Users.AnyAsync(x => x.Email.Equals(dto.Email)))
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.BadRequest, "This email is already taken");

            if (await Context.Users.AnyAsync(x => x.Email.Equals(dto.UserName)))
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.BadRequest, "This UserName is already taken");

            return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.OK);
        }

        private async Task<bool> ChangeUserImageAsync(ApplicationUser user, IFormFile image)
        {
            if (image is null || image.Length <= 0)
                return false;

            var fileExtension = Path.GetExtension(image.FileName);

            var imageFolderPath = "Uploads/Users/Photos";

            // Create folder for upload file
            Directory.CreateDirectory($"{_hostEnvironment.WebRootPath}/{imageFolderPath}");

            // Find all files named as user id, regardless of the extension
            var files = Directory.GetFiles($"{_hostEnvironment.WebRootPath}/{imageFolderPath}", $"{user.Id}.*");

            // If found any files
            if (files.Length > 0)
                // Delete them
                foreach (var file in files)
                {
                    File.Delete(file);
                }

            var newFileName = $"{user.Id}{fileExtension}";

            var filePath = Path.Combine(_hostEnvironment.WebRootPath, imageFolderPath, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            user.ImageUrl = $"/{imageFolderPath}/{newFileName}";

            return true;
        }

        private async Task<ServiceResponse<RegisterUserDtoResponse>> CreateUserAsync(RegisterUserDtoRequest dto, ApplicationUser user)
        {
            _ = await ChangeUserImageAsync(user, dto.Image);

            var result = await UserManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.OK);

            var errors = string.Join("\n", result.Errors.Select(x => x.Description));

            return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.BadRequest, errors);
        }

        private async Task<ServiceResponse<RegisterUserDtoResponse>> RegisterAdminOrTeacherAsync(RegisterUserDtoRequest dto, ApplicationUser userToRegister)
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.Forbidden);

            var createUserResponse = await CreateUserAsync(dto, userToRegister);

            if (createUserResponse.StatusCode != HttpStatusCode.OK)
                return createUserResponse;

            if (dto.Role.Equals(Role.Teacher))
                _logger.LogInformation($"Registered new teacher: {dto.Email}");

            var responseDto = Mapper.Map<RegisterUserDtoResponse>(userToRegister);

            responseDto.Token = "This is a token";

            return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        private async Task<ServiceResponse<RegisterUserDtoResponse>> RegisterStudentAsync(RegisterUserDtoRequest dto, ApplicationUser userToRegister)
        {
            if (CurrentlyLoggedUser != null && CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.BadRequest, "You need to log out first");

            var createUserResponse = await CreateUserAsync(dto, userToRegister);

            if (createUserResponse.StatusCode != HttpStatusCode.OK)
                return createUserResponse;

            _logger.LogInformation($"Registered new student: {dto.Email}");

            var responseDto = Mapper.Map<RegisterUserDtoResponse>(userToRegister);

            responseDto.Token = "This is a token";

            return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.OK, responseDto);
        }
    }
}
