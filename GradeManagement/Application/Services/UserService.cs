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
        private readonly ILogger _logger;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly IJwtGenerator _jwtGenerator;
        public UserService(IServiceProvider serviceProvider, ILogger<UserService> logger, 
            SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostEnvironment,
            IJwtGenerator jwtGenerator) : base(serviceProvider)
        {
            _logger = logger;

            _signInManager = signInManager;

            _hostEnvironment = hostEnvironment;

            _jwtGenerator = jwtGenerator;
        }

        public async Task<ServiceResponse<RegisterUserDtoResponse>> RegisterUserAsync(RegisterUserDtoRequest dto)
        {
            var validationResponse = await ValidateRegisterRequestAsync(dto);

            if (validationResponse.StatusCode != HttpStatusCode.OK)
                return validationResponse;

            var userToRegister = Mapper.Map<ApplicationUser>(dto);

            if (dto.Role.Equals(Role.Administrator) || dto.Role.Equals(Role.Teacher))
                return await RegisterAdminOrTeacherAsync(dto, userToRegister);

            return await RegisterStudentAsync(dto, userToRegister);
        }

        public async Task<ServiceResponse<LoginUserDtoResponse>> LoginUserAsync(LoginUserDtoRequest dto)
        {
            if (CurrentlyLoggedUser != null)
                return new ServiceResponse<LoginUserDtoResponse>(HttpStatusCode.BadRequest, "You need to log out first");

            var user = await UserManager.FindByEmailAsync(dto.Email);

            if (user is null)
                return new ServiceResponse<LoginUserDtoResponse>(HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return new ServiceResponse<LoginUserDtoResponse>(HttpStatusCode.Unauthorized);

            var responseDto = Mapper.Map<LoginUserDtoResponse>(user);

            responseDto.Token = _jwtGenerator.CreateToken(user, DateTime.Now.AddDays(3));

            return new ServiceResponse<LoginUserDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse<GetAllUsersDtoResponse>> GetAllUsersAsync()
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<GetAllUsersDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<GetAllUsersDtoResponse>(HttpStatusCode.Forbidden);

            var users = await Context.Users.ToListAsync();

            var responseDto = new GetAllUsersDtoResponse { Users = Mapper.Map<List<UserForGetAllUsersDtoResponse>>(users) };

            return new ServiceResponse<GetAllUsersDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse<GetUserByIdDtoResponse>> GetUserByIdAsync(string userId)
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<GetUserByIdDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator
                && CurrentlyLoggedUser.Role != Role.Teacher)
                return new ServiceResponse<GetUserByIdDtoResponse>(HttpStatusCode.Forbidden);

            var user = await Context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

            if (user is null)
                return new ServiceResponse<GetUserByIdDtoResponse>(HttpStatusCode.NotFound);

            var responseDto = Mapper.Map<GetUserByIdDtoResponse>(user);

            return new ServiceResponse<GetUserByIdDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse<GetAllStudentsDtoResponse>> GetAllStudentsAsync()
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<GetAllStudentsDtoResponse>(HttpStatusCode.Unauthorized);

            var students = await Context.Users.Where(x => x.Role.Equals(Role.Student)).ToListAsync();

            var responseDto = new GetAllStudentsDtoResponse { Students = Mapper.Map<List<StudentForGetAllStudentsDtoResponse>>(students) };

            return new ServiceResponse<GetAllStudentsDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse<GetAllTeachersDtoResponse>> GetAllTeachersAsync()
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<GetAllTeachersDtoResponse>(HttpStatusCode.Unauthorized);

            var teachers = await Context.Users.Where(x => x.Role.Equals(Role.Teacher)).ToListAsync();

            var responseDto = new GetAllTeachersDtoResponse { Student = Mapper.Map <List<TeacherForGetAllStudentsDtoResponse>>(teachers) };

            return new ServiceResponse<GetAllTeachersDtoResponse>(HttpStatusCode.OK);
        }

        public async Task<ServiceResponse<EditUserProfileDtoResponse>> EditUserProfileAsync(EditUserProfileDtoRequest dto)
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse<EditUserProfileDtoResponse>(HttpStatusCode.Unauthorized);

            var validationResponse = await ValidateEditRequestAsync(dto);

            if (validationResponse.StatusCode != HttpStatusCode.OK)
                return validationResponse;

            return await UpdateUserProfileAsync(dto);
        }

        public async Task<ServiceResponse> DeleteUserAsync(string userId)
        {
            if (CurrentlyLoggedUser is null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator &&
                CurrentlyLoggedUser.Id.Equals(userId))
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var user = await UserManager.FindByIdAsync(userId);

            if (user is null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            await ClearUserDataAsync(user);

            _logger.LogInformation($"User: {user.UserName} deleted by: {CurrentlyLoggedUserName}");

            return new ServiceResponse(HttpStatusCode.OK);
        }

        private async Task<ServiceResponse<RegisterUserDtoResponse>> ValidateRegisterRequestAsync(RegisterUserDtoRequest dto)
        {
            if (await Context.Users.AnyAsync(x => x.Email.Equals(dto.Email)))
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.BadRequest, "This email is already taken");

            if (await Context.Users.AnyAsync(x => x.UserName.Equals(dto.UserName)))
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.BadRequest, "This UserName is already taken");

            return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.OK);
        }

        private async Task<ServiceResponse<EditUserProfileDtoResponse>> ValidateEditRequestAsync(EditUserProfileDtoRequest dto)
        {
            if (await Context.Users.AnyAsync(x => x.Email.Equals(dto.Email) && x.Email != dto.Email))
                return new ServiceResponse<EditUserProfileDtoResponse>(HttpStatusCode.BadRequest, "This email is already taken");

            if (await Context.Users.AnyAsync(x => x.Email.Equals(dto.UserName) && x.UserName != dto.UserName))
                return new ServiceResponse<EditUserProfileDtoResponse>(HttpStatusCode.BadRequest, "This UserName is already taken");

            return new ServiceResponse<EditUserProfileDtoResponse>(HttpStatusCode.OK);
        }

        private async Task<bool> ChangeUserImageAsync(ApplicationUser user, IFormFile image)
        {
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

            if (image is null || image.Length <= 0)
                return false;

            var fileExtension = Path.GetExtension(image.FileName);

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
                _logger.LogInformation($"Registered new teacher: {dto.UserName}");

            var responseDto = Mapper.Map<RegisterUserDtoResponse>(userToRegister);

            responseDto.Token = _jwtGenerator.CreateToken(userToRegister, DateTime.Now.AddDays(3));

            return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        private async Task<ServiceResponse<RegisterUserDtoResponse>> RegisterStudentAsync(RegisterUserDtoRequest dto, ApplicationUser userToRegister)
        {
            if (CurrentlyLoggedUser != null && CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.BadRequest, "You need to log out first");

            var createUserResponse = await CreateUserAsync(dto, userToRegister);

            if (createUserResponse.StatusCode != HttpStatusCode.OK)
                return createUserResponse;

            _logger.LogInformation($"Registered new student: {dto.UserName}");

            var responseDto = Mapper.Map<RegisterUserDtoResponse>(userToRegister);

            responseDto.Token = _jwtGenerator.CreateToken(userToRegister, DateTime.Now.AddDays(3));

            return new ServiceResponse<RegisterUserDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        private async Task ClearUserDataAsync(ApplicationUser user)
        {
            var notes = await Context.Notes.Where(x => x.UserId.Equals(user.Id)).ToListAsync();

            Context.Notes.RemoveRange(notes);

            var sentMessages = await Context.Messages.Where(x => x.UserFromId.Equals(user.Id)).ToListAsync();

            foreach (var message in sentMessages)
                message.UserFromId = null;

            var receivedMessages = await Context.Messages.Where(x => x.UserToId.Equals(user.Id)).ToListAsync();

            foreach (var message in receivedMessages)
                message.UserToId = null;

            var messagesToRemove = sentMessages
                .Where(x => x.UserFromId is null && x.UserToId is null)
                .ToList();

            messagesToRemove.AddRange(receivedMessages
                .Where(x => x.UserToId is null && x.UserFromId is null)
                .ToList());

            Context.RemoveRange(messagesToRemove);

            await Context.SaveChangesAsync();

            _ = await ChangeUserImageAsync(user, null);

            await UserManager.DeleteAsync(user);
        }

        private async Task<ServiceResponse<EditUserProfileDtoResponse>> UpdateUserProfileAsync(EditUserProfileDtoRequest dto)
        {
            Mapper.Map(dto, CurrentlyLoggedUser);

            if (dto.Image != null)
            {
                var isImageChanged = await ChangeUserImageAsync(CurrentlyLoggedUser, dto.Image);

                if (isImageChanged)
                    await Context.SaveChangesAsync();
            }

            var updateResponse = await UserManager.UpdateAsync(CurrentlyLoggedUser);

            var responseDto = Mapper.Map<EditUserProfileDtoResponse>(CurrentlyLoggedUser);

            return updateResponse.Equals(IdentityResult.Success)
                ? new ServiceResponse<EditUserProfileDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<EditUserProfileDtoResponse>(HttpStatusCode.BadRequest, "Unable to update user");
        }
    }
}
