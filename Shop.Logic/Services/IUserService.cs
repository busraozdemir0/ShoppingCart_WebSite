using Shop.DataModels.CustomModels;
using Shop.DataModels.Models;
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
        List<CustomerOrder> GetOrdersByCustomerId(int customerId);
        List<CartModel> GetOrderDetailForCustomer(int customerId, string order_number);
        ResponseModel ChangePassword(PasswordModel passwordModel);
        List<string> GetShippingStatusForOrder(string order_number);
        Task<string> MakePaymentStripe(string cardNumber, int expMonth, int expYear, string cvc, decimal value); // Kartla Stripe yontemi ile odeme
    }
}
