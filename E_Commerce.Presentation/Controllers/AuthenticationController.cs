using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.DTOS.IdentityDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
