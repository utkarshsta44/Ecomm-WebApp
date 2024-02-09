using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
      


        public HomeController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public  IActionResult Index()
        {

            var data =  _applicationDbContext.Products.Where(x => x.IsAvailable == true).ToList();
            ViewBag.CategoryList = _applicationDbContext.Categories.ToList();
            return View(data);
          
        }

        public IActionResult Details()
        {

            var data = _applicationDbContext.Products.Where(x => x.IsAvailable == true).ToList();
            ViewBag.CategoryList = _applicationDbContext.Categories.ToList();
            return View(data);

        }

        public IActionResult Privacy(string myData)
        {
            if (myData == "all")
            {
                return Json(new { result = "success" });
            }
            int categoryId = _applicationDbContext.Categories.FirstOrDefault(x => x.CategoryName.ToLower() == myData).CategoryId;
            TempData["CategoryId"] = categoryId;
            return Json(new { result = "success" });

        }

        public IActionResult GetProductsByCategoryPriceAndName(int categoryId, int? priceRange, string productName)
        {
            var products = _applicationDbContext.Products.Include(x => x.Category)
                                            .Where(x => (categoryId == 0 || x.ProductCategoryId == categoryId) &&
                                                        (priceRange == null || x.ProductPrice <= priceRange) &&
                                                        (string.IsNullOrEmpty(productName) || x.ProductName.Contains(productName)) &&
                                                        x.IsAvailable == true)
                                            .OrderByDescending(x => x.ProductCreatedAt)
                                            .ToList();

            return PartialView("_ProductByCategoryPartial", products);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
