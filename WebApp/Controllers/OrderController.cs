using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Data;
using WebApp.Models.Domain;


namespace WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly Product _product;
        private readonly ShoppingCartItem _shoppingCartItem;
        private readonly Order _order;
        private readonly ApplicationDbContext _context;



        public OrderController(Product product, ShoppingCartItem shoppingCartItem, Order order,ApplicationDbContext context)
        {
            _product = product;
            _shoppingCartItem = shoppingCartItem;
            _order = order;
            _context = context;
        }   
     

       
      /*  public async Task<ActionResult> Index()
        {
            string userid=User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userrole = User.FindFirstValue(ClaimTypes.Role);
            var order=await _order.GetOrderByUserIdAndRoleAync( userid, userrole);
            return View(order);

        }*/

    }
}
