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
    public class UserService : IUserService
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
            List<ProductModel> _productList = new List<ProductModel>();
            foreach (var p in data) // İlgili kategoriye ait urunleri nesne esleme yontemi kullanilarak model nesnesine aktariliyor.
            {
                ProductModel _productModel = new ProductModel();
                _productModel.Id = p.Id;
                _productModel.Name = p.Name;
                _productModel.Price = p.Price;
                _productModel.ImageUrl = p.ImageUrl;
                _productModel.Stock = p.Stock;
                _productList.Add(_productModel);
            }

            return _productList;
        }

        public ResponseModel RegisterUser(RegisterModel registerModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var exist_check = _dbContext.Customers.Where(x => x.Email == registerModel.EmailId).Any(); // Girilen email veri tabaninda hic var mi (onceden kaydolunmus mu?)

                if (!exist_check) // Eger kaydolmamissa
                {
                    Customer _customer = new Customer();
                    _customer.Name = registerModel.Name;
                    _customer.MobileNo = registerModel.MobileNo;
                    _customer.Email = registerModel.EmailId;
                    _customer.Password = registerModel.Password;
                    _dbContext.Add(_customer);
                    _dbContext.SaveChanges();

                    LoginModel loginModel = new LoginModel()
                    {
                        EmailId = registerModel.EmailId,
                        Password = registerModel.Password,
                    };

                    response = LoginUser(loginModel);
                }
                else // Eger ayni mail ile kaydolmussa
                {
                    response.Status = false;
                    response.Message = "Email already registered.";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "An Error has occured. Please try again!";
               
                return response;
            }
        }

        public ResponseModel LoginUser(LoginModel loginModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var userData = _dbContext.Customers.Where(x => x.Email == loginModel.EmailId).FirstOrDefault();
                if (userData != null)
                {
                    if (userData.Password == loginModel.Password)
                    {
                        response.Status = true;
                        response.Message = Convert.ToString(userData.Id) + "|" + userData.Name + "|" + userData.Email;
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = "Your Password in Incorrect";
                    }
                }
                else
                {
                    response.Status = false;
                    response.Message = "Email not registered. Please check your Email Id";
                }
                return response;

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "An Error has occured. Please try again!";

                return response;
            }
        }
    }
}
