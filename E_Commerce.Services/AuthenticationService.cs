using E_Commerce.Domain.Entityes.Identity_Module;
using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.Common_Responses;
using E_Commerce.Shared.DTOS.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IConfiguration _Configuration { get; }

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _Configuration = configuration;
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
            var token = await CreateTokenAsync(User);
            return new UserDTO(User.Email!, User.DisplayName, token);
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
            {
                var token = await CreateTokenAsync(User);
                return new UserDTO(User.Email, User.DisplayName, token);
            }
                
            //If Error
            return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();
        }

        //https://chatgpt.com/share/6add143a-409c-83ea-85cd-8e0ef5923359  Explain JWT 
        //https://www.jwt.io/  بيعمل decoding For Token
        public async Task<string> CreateTokenAsync(ApplicationUser user)
        {

            //Download Package To Generate Tockens [in Services Layer] => اسمها JWT bearer
            //JWT يتكون من Header دى بتتعمل لوحدها + Payload انا اللى بعمله  + Signature



            //Token مكونة من Issuer + Audience + Claims[Private] + ExpiredData+ SigninCrediention[SecretKey]

            var claims = new List<Claim>()//Generate Private Claims
            {

                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName!),

            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }//كل دا عشان اعمل للUser اللى عاملRegister عايز اعمله token عشان يستخدمه فى الLogin

            //Credentioal has Secret Key سواء Asymetric of Symtreic Key + EncondingAlgorithm

            //this Key For Credntitials
            //var secretkey = "MySecretKey";//دى معلومة بتتغير من Enviorment للتانيه
            var secretkey = _Configuration["JwtOptions:SecretKey"]!;//بقيت اقراها من app Setting
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            //this Algorithm
            var Cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//Use Symetreic Key


            var tocken = new JwtSecurityToken(
                //هبدا ب Registered Claims   as Header Claimsبيتعمل لوحده فانا ببدا بعمل ال
                //issuer: "MyIssuer",//مين اللى عمل الToken  //دى معلومة بتتغير من Enviorment للتانيه
                issuer: _Configuration["JwtOptions:Issuer"],
                //audience: "MyAudience",//دى معلومة بتتغير من Enviorment للتانيه
                audience: _Configuration["JwtOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(1),//بعد ساعة واحدة يكون Expired واللى بيحددها هو Buissnes Domain
                claims: claims,
                signingCredentials: Cred
                );

            //return token as String After Validation For Token
            return new JwtSecurityTokenHandler().WriteToken(tocken);

        }



    }
}
