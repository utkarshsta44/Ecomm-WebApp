using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models.Domain;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Models.Dto;

namespace WebApp.Controllers
{
    public class UserServicesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;

        public UserServicesController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, SignInManager<UserModel> signInManager,
        UserManager<UserModel> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _userManager= userManager;  

            _signInManager = signInManager;
        }

        private void MapImagePaths(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
               
                product.ProductImage = Url.Content($"~/Content/images/{product.ProductImage}");
            }
        }
        public IActionResult Represent()
        {

            var products = _applicationDbContext.Products
                 .Include(p => p.Category);
            MapImagePaths(products);

            return View(products);
        }
        public IActionResult Display()
        {

            var data = _applicationDbContext.Products.ToList();
            return View(data);
          
        }
        public  IActionResult UserProfileImprover(string id)
        {

            var user = _applicationDbContext.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                var newUser = new UpdateUserDto()
                {
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNo = user.PhoneNo,
                    UserPassword = user.UserPassword,
                    CreatedAt = DateTime.UtcNow
                };
                return View("UserProfileImprover", newUser);
            }
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                var newUser = new UpdateUserDto()
                {
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNo = user.PhoneNo,
                    UserPassword = user.UserPassword,
                    CreatedAt = DateTime.UtcNow
                };
                return await Task.Run(() => View("UpdateUser", newUser));
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == updateUserDto.Id);

            if (user != null)
            {

                user.Name = updateUserDto.Name;
                user.PhoneNo = updateUserDto.PhoneNo;
                user.Email = updateUserDto.Email;

                user.CreatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(updateUserDto.UserPassword))
                {

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetPasswordResult = await _userManager.ChangePasswordAsync(user, user.UserPassword, updateUserDto.UserPassword);

                    if (resetPasswordResult.Succeeded)
                    {
                        user.UserPassword = updateUserDto.UserPassword;
                        user.ConfirmPassword = updateUserDto.UserPassword;

                        await _userManager.UpdateAsync(user);
                    }

                }

            }
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult DeactiveUser(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = false;
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Home", "Index");
        }
        [HttpGet]
        public IActionResult ActivateUser(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = true;
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Home", "Index");
        }
    }

}