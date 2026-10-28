using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.Common_Responses;
using E_Commerce.Shared.DTOS.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
  
    public class AuthenticationController:ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        //First Endpiont=> Login 
        //Post : Baseurl/Api/Authentication/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>>Login(LoginDto loginDto)
        {
           var Result=await _authenticationService.LoginAsync(loginDto);
           return HandleResult(Result);

        }

        //Second Endpiont => Register
        //post Baseurl/api/Authentication/Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>>Register(RegisterDTO registerDto)
        {
            var result=await _authenticationService.RegisterAsync(registerDto);
            return HandleResult(result);
        }


        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
           var result= await _authenticationService.CheckEmailAsync(email);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
             var email=User.FindFirstValue(ClaimTypes.Email);//عشان لو الtoken اللى انا عامله شايل اكتر من ايميل فى الPrivate Claims اللى انا عاملها 
            //كمان ببعت الClaims عشان يعمل Validation For Token اللى مبعوت 
            var result=await _authenticationService.GetUserByEmailAsync(email!);
            return HandleResult(result);
        }
    }
}
