using E_Commerce.Domain.Entityes.Identity_Module;
using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.Common_Responses;
using E_Commerce.Shared.DTOS.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDto loginDto)
        {
            //Check Valid Email is Exsit in Database 
            //then Check Password
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User is null)
                return Error.InvalidCredentials("User.InvalidCredentials");
            bool IsPassValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);//بيقارن ويتاكد ان الPass اللى انت مدخله هو اللى موجود فى الDatabase 
            if (!IsPassValid)
                return Error.InvalidCredentials("User.InvalidCredentials");
            return new UserDTO(User.Email!, User.DisplayName, "Token");
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
            };
            var IdentityResult = await _userManager.CreateAsync(User, registerDTO.Password);
            if (IdentityResult.Succeeded)
                return new UserDTO(User.Email, User.DisplayName, "Token");
            //If Error
            return IdentityResult.Errors.Select(E=>Error.Validation(E.Code,E.Description)).ToList();
        }
    }
}
