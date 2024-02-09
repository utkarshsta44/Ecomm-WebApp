using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp.Data;
using WebApp.Models.Domain;
using WebApp.Models.Dto;

namespace WebApp.Controllers
{
   
    public class FavouriteController : Controller
    {
        /* public IActionResult Index()
         {
             return View();
         }*/


        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        public FavouriteController(ApplicationDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {

            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var favouriteProducts = _context.Favourites
                .Where(x => x.UsersId == userId)
                .Join(
                    _context.Products,
                    favouriteItem => favouriteItem.ProductId,
                    product => product.ProductId,
                    (favouriteItem, product) => new Favourites
                    {
                        FId = favouriteItem.FId,
                        ProductId = favouriteItem.ProductId,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        ProductImage = product.ProductImage,
                        ProductDescription = product.ProductDescription,

                    }
                )
                .ToList();

            return View(favouriteProducts);
        }

        public async Task<IActionResult> AddToFavourite(int id)
        {
            //if (_signInManager.IsSignedIn(User))
            //{
            //    if (id != 0)
            //    {

            //        var user = _signInManager.UserManager.GetUserId(User);

            //        var existItem = _context.Favourites;

            //        if (existItem == null)
            //        {
            //            var item = new Favourite()
            //            {
            //                ProductId = id,
            //                UsersId = user,


            //            };
            //            await _context.Favourites.AddAsync(item);
            //            //await _context.SaveChangesAsync();
            //        }
            //       else
            //        {

            //            //_context.Favourites.Remove(existItem);

            //        }
            //       await _context.SaveChangesAsync();
            //    }
            //    return RedirectToAction("Index");
            //}

            //return RedirectToAction("Login", "Auth");
            if (_signInManager.IsSignedIn(User))
            {
                if (id != 0)
                {
                    var user = _signInManager.UserManager.GetUserId(User);
                    var userId = _signInManager.UserManager.FindByIdAsync(user).Result.UserName;
                    var existingItem = _context.Favourites.Where(x => x.UsersId == user).FirstOrDefault(x => x.ProductId == id);

                    if (existingItem != null)
                    {
                        TempData["favProduct"] = "This Product is Already added in Favorites";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var newFav = new Favourite()
                        {
                            ProductId = id,
                            UsersId = user,
                        };
                        await _context.Favourites.AddAsync(newFav);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Favourite");
                }
                return RedirectToAction("Index", "Home");
            }
      /*      [("Kindly Login first")]*/
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFavourite(int id)
        {
            if (id != null)
            {
                var item = _context.Favourites.FirstOrDefault(x => x.FId == id);
                if (item != null)
                {
                    _context.Favourites.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Favourite");
        }
    }
}
