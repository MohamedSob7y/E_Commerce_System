using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes.Identity_Module;
using E_Commerce.Persistance.Data.DataSeeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.IdentityData.Data_Seed
{
    public class IdentityDataInitializer : IDataSeeding
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //Ask CLR to inject object From ApplicationUsers in Runtime For 2 Service 
        public IdentityDataInitializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            ,ILogger<IdentityDataInitializer> logger)
        {
            _userManager = userManager;
            _Logger = logger;
            _RoleManager = roleManager;
        }

        public RoleManager<IdentityRole> _RoleManager { get; }
        public ILogger _Logger { get; }

        public async Task IntializeAsync()
        {
            try
            {
                //1: Create Roles فى حالة ان مفيش tables Role
                if (!_RoleManager.Roles.Any())
                {
                    await _RoleManager.CreateAsync(new IdentityRole("Admin"));
                    await _RoleManager.CreateAsync(new IdentityRole("SuperAdmin"));//Create Roles
                }
                //2: Create Users
                if (!_userManager.Users.Any())
                {

                    //دول هما الadmin بتوع الsystem
                    var User01 = new ApplicationUser()
                    {
                        DisplayName = "Omar Ahmed",
                        UserName = "OmarAhmed",
                        Email = "OmarAhmed@gmail.com",
                        PhoneNumber = "01070995586"
                    };
                    var user02 = new ApplicationUser()
                    {
                        DisplayName = "Farida Ahmed",
                        UserName = "FaridaAhmed",
                        Email = "FaridaAhmed@gmail.com",
                        PhoneNumber = "01070995486"
                    };
                    await _userManager.CreateAsync(User01, "P@ssw0rd");//Send User + Send Password وهو اللى هيعمل الHashing then Store it in Database 
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    //3: Assign Roles اللى انا عمتلها على IUsers اللى انا عملتهم 
                    //اللى متحكم هو الUserManger 
                    await _userManager.AddToRoleAsync(User01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(user02, "Admin");

                }


            }
            catch (Exception Ex)
            {

                _Logger.LogError($"Error While Seeding Database {Ex.Message} Happen");
            }
        }
    }
}
