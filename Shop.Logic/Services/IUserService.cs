using Shop.DataModels.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Services
{
    public interface IUserService
    {
        List<CategoryModel> GetCategories();
        Task<List<ProductModel>> GetProductByCategoryId(int categoryId);
        ResponseModel RegisterUser(RegisterModel registerModel); // Ana sayfa uzerinden kayit olan kullanicilar
        ResponseModel LoginUser(LoginModel loginModel);
        ResponseModel Checkout(List<CartModel> cartItems); // Siparis verme islemleri
    }
}
