using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp.Data;
using WebApp.Models.Domain;
using WebApp.Models.Dto;

namespace WebApp.Controllers

{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context, SignInManager<UserModel> signInManager,
          UserManager<UserModel> userManager,
         RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        [HttpGet]
        public async Task<IActionResult> AdminHome()
        {
            //GetAll
            var allProducts = _context.Products.ToList();
            var productList = await _context.Products.ToListAsync();
            var categoryList = await _context.Categories.ToListAsync();
            var categoryProduct = productList.Join(// outer sequence 
                       categoryList,  // inner sequence 
                       product => product.ProductCategoryId,   // outerKeySelector
                       category => category.CategoryId, // innerKeySelector
                       (product, category) => new CategoryProductDto // result selector
                       {
                           ProductId = product.ProductId,
                           ProductName = product.ProductName,
                           ProductDescription = product.ProductDescription,
                           ProductPrice = product.ProductPrice,
                           ProductImage = product.ProductImage,
                           IsAvailable = product.IsAvailable,
                           IsActive = product.IsActive,
                           ProductCreatedAt = product.ProductCreatedAt,
                           IsTrending = product.IsTrending,
                           CategoryId = category.CategoryId,
                           CategoryName = category.CategoryName
                       }).OrderByDescending(x => x.ProductCreatedAt).ToList();
            ViewBag.CategoryList = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategory = "fashion";
            return View(categoryProduct);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allProducts = _context.Products.ToList();
            var productList = await _context.Products.ToListAsync();
            var categoryList = await _context.Categories.ToListAsync();
            var categoryProduct = productList.Join(// outer sequence 
                       categoryList,  // inner sequence 
                       product => product.ProductCategoryId,   // outerKeySelector
                       category => category.CategoryId, // innerKeySelector
                       (product, category) => new CategoryProductDto // result selector
                       {
                           ProductImage = product.ProductImage,
                           ProductId = product.ProductId,
                           ProductName = product.ProductName,
                           ProductDescription = product.ProductDescription,
                           ProductPrice = product.ProductPrice,
                          
                           IsAvailable = product.IsAvailable,
                           IsActive = product.IsActive,
                           ProductCreatedAt = product.ProductCreatedAt,
                           IsTrending = product.IsTrending,
                           CategoryId = category.CategoryId,
                           CategoryName = category.CategoryName
                       }).OrderByDescending(x => x.ProductCreatedAt).ToList();

            return Json(new { data = categoryProduct });
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

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
            return RedirectToAction("GetAllUser", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == updateUserDto.Id);

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
            await _context.SaveChangesAsync();
            return RedirectToAction("GetAllUser", "Auth");
        }
        public async Task<IActionResult> AdminProfileImprover()
        {
            return View();
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
            _context.SaveChanges();
            return RedirectToAction("GetAllUser", "Auth");
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
            _context.SaveChanges();
            return RedirectToAction("GetAllUser", "Auth");
        }
    }
}