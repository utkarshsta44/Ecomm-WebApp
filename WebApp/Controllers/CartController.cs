
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp.Data;
using WebApp.Models.Domain;


namespace WebApp.Controllers
{

/*    [Authorize(Roles = "User")]
*/    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        public CartController(ApplicationDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
               
           _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }




        public IActionResult Index()
        {
            var userId = _signInManager.UserManager.GetUserId(User);
            var cartItem = _context.Carts.Include(X => X.Product).Where(x => x.UserId1 == userId).ToList();
          

            
            
            return View(cartItem);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {



                if (id != null)
                {
                    /*var existingCart = _context.Carts.FirstOrDefault(x => x.UserId1 == id);
                    var user = _signInManager.UserManager.GetUserId(User);

*/
                    var user = _signInManager.UserManager.GetUserId(User);
                    var userId = _signInManager.UserManager.FindByIdAsync(user).Result.UserName;

                    var existingCart = _context.Carts.Where(x => x.UserId1 == user).FirstOrDefault(x => x.ProductId == id);

                    if (existingCart != null)
                    {
                        existingCart.CartItems++;
                    }
                    else
                    {
                        var newcart = new Cart()
                        {
                         
                            CartItems = 1,
                            ProductId = id,
                            UserId = 24,
                            UserId1 = user
                        };
                        await _context.Carts.AddAsync(newcart);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("UserHomePage", "User");
            }
            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
                if (id != null)
                {
                    var existingCart = _context.Carts.FirstOrDefault(x => x.ProductId == id);
                    var user = _signInManager.UserManager.GetUserId(User);


                    if (existingCart != null)
                    {
                        if(existingCart.CartItems== 1)
                         {
                             _context.Carts.Remove(existingCart);
                         }
                         else
                         {
                             existingCart.CartItems--;
                         }
                        await _context.SaveChangesAsync();
                    }
                   

                    return RedirectToAction("Index");
                }
                return RedirectToAction("UserHomePage", "User");
          
        }








        /* [HttpPost]
             public async Task<IActionResult> AddToCart(int id)
             {
                 if (_signInManager.IsSignedIn(User))
                 {

                     if (id!=0)
                     {
                         var existingCart = _context.Carts.FirstOrDefault(x => x.ProductId == id);
                *//*     var user = _signInManager.UserManager.GetUserId(User);
                     var userId = _signInManager.UserManager.FindByIdAsync(user).Result.UserName;*//*

                     var user1 = _context.Users.FirstOrDefault(u=>u.UserName==User.Identity.Name);

                     if (existingCart != null)
                         {
                             existingCart.CartItems++;
                         }
                         else
                         {
                         var newcart = new Cart()
                         {

                             CartItems = 1,
                             ProductId = id,
                             User = user1

                             };
                             await _context.Carts.AddAsync(newcart);
                         }
                         await _context.SaveChangesAsync();
                         return RedirectToAction("Index","Cart");
                     }
                     return RedirectToAction("Index", "Home");
                 }
                 return RedirectToAction("Login", "Authentication");
             }

             [HttpGet]
             public async Task<IActionResult> OrderDelet([FromRoute] int id)
             {
                 if (id != 0)
                 {
                     var CartItems = _context.Carts.FirstOrDefault(x => x.CartId == id);
                     if (CartItems != null)
                     {
                         _context.Carts.Remove(CartItems);
                         await _context.SaveChangesAsync();
                     }
                 }
                 return RedirectToAction("Index");
             }*/
    }
    }









