using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models.Dto;
using System.Data;
using WebApp.Constant;
using WebApp.Models.Domain;
using Azure.Core;

namespace WebApp.Controllers
{

    public class AuthController : Controller
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(SignInManager<UserModel> productDbContext, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = productDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AdminHome", "Admin");
                }
                if (User.IsInRole("User"))
                {
                    return RedirectToAction("Display", "UserServices");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            /* var user =  _userManager.Users.FirstOrDefault(x=>x.Email==model.UserEmail);*/
         

            bool user = _userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserEmail).Result.IsActive;
            
            
                
            
            if (ModelState.IsValid)
            {
                if (user)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.UserEmail!, model.UserPassword!, false, false);          //remember me not working

                    if (result.Succeeded)
                    {
                        if (User.IsInRole("Admin"))
                        {
                            TempData["success1"] = "You have  been successfully Logged In  As Admin!";

                            return RedirectToAction("AdminHome", "Admin");
                        }
                        if (User.IsInRole("User"))
                        {
                            TempData["success1"] = "You have  been successfully Logged In!";

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    TempData["deactiveLogin"] = "Your account has been deactivated by the admin!";
                    return RedirectToAction("Login", "Auth");
                }
                //login

               
            }
           ModelState.AddModelError("", "Invalid login attempt ");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(AddUserDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel
                {

                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNo = model.PhoneNo,
                    UserPassword = model.UserPassword,
                    ConfirmPassword = model.ConfirmPassword,
                    CreatedAt = DateTime.Now

                };

                var result = await _userManager.CreateAsync(user, model.UserPassword!);

                if (result.Succeeded)
                {                   
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("GetAllUser", "Auth");
                    }
                    await _userManager.AddToRoleAsync(user, Roles.User.ToString()!);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "You have  been successfully Logged In!";

                    return RedirectToAction("Index", "Home");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "You have  been successfully Logged out!";

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            var users = _userManager.Users;
            return View(users);
        }


    }
}