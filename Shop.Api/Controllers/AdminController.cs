using Microsoft.AspNetCore.Mvc;
using Shop.DataModels.CustomModels;
using Shop.Logic.Services;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly IAdminService _adminService;

        public AdminController(IWebHostEnvironment env, IAdminService adminService)
        {
            this.env = env;
            _adminService = adminService;
        }

        [HttpPost]
        [Route("AdminLogin")]
        public IActionResult AdminLogin(LoginModel loginModel)
        {
            var data = _adminService.AdminLogin(loginModel);
            return Ok(data);
        }

        [HttpPost]
        [Route("SaveCategory")]
        public IActionResult SaveCategory(CategoryModel newCategory)
        {
            var data = _adminService.SaveCategory(newCategory);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCategories")]
        public IActionResult GetCategories()
        {
            var data = _adminService.GetCategories();
            return Ok(data);
        }

        [HttpPost]
        [Route("UpdateCategory")]
        public IActionResult UpdateCategory(CategoryModel categoryToUpdate)
        {
            var data = _adminService.UpdateCategory(categoryToUpdate);
            return Ok(data);
        }

        [HttpPost]
        [Route("DeleteCategory")]
        public IActionResult DeleteCategory(CategoryModel categoryToDelete)
        {
            var data = _adminService.DeleteCategory(categoryToDelete);
            return Ok(data);
        }


        [HttpGet]
        [Route("GetProducts")]
        public IActionResult GetProducts()
        {
            var data = _adminService.GetProducts();
            return Ok(data);
        }

        [HttpPost]
        [Route("SaveProduct")]
        public IActionResult SaveProduct(ProductModel newProduct)
        {
            int nextProductId = _adminService.GetNewProductId();
            newProduct.ImageUrl = @"Images/" + nextProductId + ".png";
            var path = $"{env.WebRootPath}\\Images\\{nextProductId + ".png"}";
            var fs = System.IO.File.Create(path);
            fs.Write(newProduct.FileContent, 0, newProduct.FileContent.Length);
            fs.Close();

            string uploadsFolder = Path.Combine(env.WebRootPath, "Images");

            var data = _adminService.SaveProduct(newProduct);
            return Ok(data);
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct(ProductModel productToDelete)
        {
            var data = _adminService.DeleteProduct(productToDelete);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetProductStock")]
        public IActionResult GetProductStock() // Tum urun adlarini stoklariyla birlikte getirme
        {
            var data = _adminService.GetProductStock();
            return Ok(data);
        }

        [HttpPost]
        [Route("UpdateProductStock")]  // Urunun stogunu guncelleme islemi
        public IActionResult UpdateProductStock(StockModel stock)
        {
            var data = _adminService.UpdateProductStock(stock);
            return Ok(data);
        }
    }
}
