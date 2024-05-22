using Microsoft.EntityFrameworkCore;
using Shop.DataModels.CustomModels;
using Shop.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Services
{
    public class UserService:IUserService
    {
        private readonly ShoppingCartDBContext _dbContext = null;

        public UserService(ShoppingCartDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<CategoryModel> GetCategories()
        {
            var data = _dbContext.Categories.ToList();
            List<CategoryModel> _categoryList = new List<CategoryModel>();
            foreach (var c in data)
            {
                CategoryModel _categoryModel = new CategoryModel();
                _categoryModel.Id = c.Id;
                _categoryModel.Name = c.Name;
                _categoryList.Add(_categoryModel);
            }
            return _categoryList;
        }

        public async Task<List<ProductModel>> GetProductByCategoryId(int categoryId)
        {
            var data = _dbContext.Products.Where(x => x.CategoryId == categoryId).ToList(); // Gelen kategori id'ye esit olan urunler listeleniyor.
            List<ProductModel> _productList=new List<ProductModel>();
            foreach(var p in data) // İlgili kategoriye ait urunleri nesne esleme yontemi kullanilarak model nesnesine aktariliyor.
            {
                ProductModel _productModel = new ProductModel();
                _productModel.Id = p.Id;
                _productModel.Name = p.Name;
                _productModel.Price= p.Price;
                _productModel.ImageUrl = p.ImageUrl;
                _productModel.Stock = p.Stock;
                _productList.Add(_productModel);
            }

            return _productList;
        }
    }
}
