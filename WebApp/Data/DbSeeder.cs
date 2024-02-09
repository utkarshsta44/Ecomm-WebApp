using Microsoft.AspNetCore.Identity;
using WebApp.Models.Domain;
using System.Data;
using WebApp.Constant;


namespace WebApp.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<UserModel>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();


            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // creating new admin

            var user = new UserModel
            {
                UserName = "admin123@gmail.com",
                Name = "Utkarsh srivastava",
                Email = "admin@gmail.com",
                UserPassword = "Admin@123",
                ConfirmPassword = "Admin@123",
                Role = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedAt = DateTime.Now
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }
    }
}