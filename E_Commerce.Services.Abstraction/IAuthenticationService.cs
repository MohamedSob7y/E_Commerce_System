using E_Commerce.Shared.Common_Responses;
using E_Commerce.Shared.DTOS.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Abstraction
{
    public interface IAuthenticationService
    {
        //Login => Take Email+ Password return Token + DisplayName+Email
        Task<Result<UserDTO>>LoginAsync(LoginDto loginDto);
        //Register=> Take Email + UserName + DisplayName+PhoneNumber return Token + DisplayName+ Email
        //انا محتاج DTO واحد للاتنين عشان نفس الreturn 
        //محتاج DTO For Take Login
        //Dto For Take Register
        Task<Result<UserDTO>>RegisterAsync(RegisterDTO registerDTO);

        //CheckEmail=> Check That Email of Users is Exsist or Not
        Task<bool> CheckEmailAsync(string email);

        //Check Current User=> by GetUserByEmail
        Task<Result<UserDTO>> GetUserByEmailAsync(string Email);

    }
}
