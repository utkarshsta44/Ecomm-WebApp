using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Domain;
using System;
using WebApp.Data;
using WebApp.Models.Dto;
using Microsoft.AspNetCore.Authorization;


namespace WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productList = await _applicationDbContext.Products.ToListAsync();
            var categoryList = await _applicationDbContext.Categories.ToListAsync();
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
            return View(categoryProduct);
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.CategoryList = await _applicationDbContext.Categories.ToListAsync();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDto addProductDto)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = "";
                if (addProductDto.ProductImage != null)
                {
                    string uploadFoler = Path.Combine(_webHostEnvironment.WebRootPath, "image");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + addProductDto.ProductImage.FileName;
                    string filePath = Path.Combine(uploadFoler, uniqueFileName);
                    addProductDto.ProductImage.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                var product = new Product
                {
                    ProductName = addProductDto.ProductName,
                    ProductDescription = addProductDto.ProductDescription,
                    ProductPrice = addProductDto.ProductPrice,
                    IsAvailable = true,

                    IsTrending = addProductDto.IsTrending,
                    ProductCreatedAt = DateTime.Now,
                    ProductCategoryId = addProductDto.ProductCategoryId,
                    ProductImage = uniqueFileName
                };
                _applicationDbContext.Add(product);
                await _applicationDbContext.SaveChangesAsync();
                ViewBag.Success = "Product added successfully";
                TempData["success"] = "Product Added Successfully";

                return RedirectToAction("AdminHome", "Admin");
            }
            else
            {
                // Display a message indicating that the form is not valid
                TempData["error"] = "Please fill in all required fields.";
            }
            return View(addProductDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            ViewBag.CategoryList = await _applicationDbContext.Categories.ToListAsync();
            if (product != null)
            {
                var newProduct = new Models.Dto.UpdateProductDto()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductDescription = product.ProductDescription,
                    IsAvailable = product.IsAvailable,
                    IsTrending = product.IsTrending,
                    //ProductImage=product.ProductImage,
                    ProductCategoryId = product.ProductCategoryId,
                    Category = product.Category,
                };
                return await Task.Run(() => View("Update", newProduct));
            }
            return RedirectToAction("AdminHome", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            var product = await _applicationDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == updateProductDto.ProductId);
            ViewBag.CaterogyList = _applicationDbContext.Categories.ToList();

            string uniqueFileName = "";
            if (updateProductDto.ProductImage != null)
            {
                string uploadFoler = Path.Combine(_webHostEnvironment.WebRootPath, "image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + updateProductDto.ProductImage.FileName;
                string filePath = Path.Combine(uploadFoler, uniqueFileName);
                updateProductDto.ProductImage.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            if (product != null)
            {
                product.ProductName = updateProductDto.ProductName;
                product.ProductPrice = updateProductDto.ProductPrice;
                product.ProductDescription = updateProductDto.ProductDescription;
                product.ProductImage = uniqueFileName;
                product.IsAvailable = updateProductDto.IsAvailable;
                product.IsTrending = updateProductDto.IsTrending;
                product.ProductCategoryId = updateProductDto.ProductCategoryId;
            }
            await _applicationDbContext.SaveChangesAsync();
            TempData["success"] = "Product Updated Successfully";
            return RedirectToAction("AdminHome", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Active(int id)
        {
            var product = _applicationDbContext.Products.FirstOrDefault(x => x.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            product.IsAvailable = true;
            await _applicationDbContext.SaveChangesAsync();
            TempData["success"] = "Product Activated Successfully";
            return RedirectToAction("Adminhome", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Deactive(int id)
        {
            var product = _applicationDbContext.Products.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            product.IsAvailable = false;
            await _applicationDbContext.SaveChangesAsync();
            TempData["successDeactive"] = "Product DeActivated Successfully";
            return RedirectToAction("AdminHome", "Admin");
        }
    


    }


}