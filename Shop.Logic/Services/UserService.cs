using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop.DataModels.CustomModels;
using Shop.DataModels.Models;
using Stripe;
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
                    Shop.DataModels.Models.Customer _customer = new Shop.DataModels.Models.Customer();
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

        // Siparis verme islemleri
        public ResponseModel Checkout(List<CartModel> cartItems)
        {
            string OrderId = GenerateOrderId();
            var prods = _dbContext.Products.ToList();

            try
            {
                // Siparisi veren kullanicinin bilgilerini CustomerOrder tablosuna kaydetme
                var detail = cartItems.FirstOrDefault();
                CustomerOrder _customerOrder = new CustomerOrder();
                _customerOrder.CustomerId = detail.UserId;
                _customerOrder.OrderId = OrderId;
                _customerOrder.PaymentMode = detail.PaymentMode;
                _customerOrder.ShippingAddress = detail.ShippingAddress;
                _customerOrder.ShippingCharges = detail.ShippingCharges; // Kargo ucreti
                _customerOrder.SubTotal = detail.SubTotal;
                _customerOrder.Total = detail.ShippingCharges + detail.SubTotal;
                _customerOrder.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy"); // Siparisin olusturuldugu tarih
                _customerOrder.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                _dbContext.CustomerOrders.Add(_customerOrder);

                // Siparis detaylarini OrderDetails tablosuna kaydetme
                foreach (var items in cartItems) // sipariste kac tane urun varsa
                {
                    OrderDetail _orderDetail = new OrderDetail();
                    _orderDetail.OrderId = OrderId;
                    _orderDetail.ProductId = items.ProductId;
                    _orderDetail.Quantity = items.Quantity;
                    _orderDetail.Price = items.Price;
                    _orderDetail.SubTotal = items.Price * items.Quantity;
                    _orderDetail.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy"); // Siparisin olusturuldugu tarih
                    _orderDetail.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                    _dbContext.OrderDetails.Add(_orderDetail);

                    // Siparisi verilen urunun stok miktarindan urunun siparis verildigi miktar kadarini eksiltme ve guncelleme islemi.
                    var selected_product = prods.Where(x => x.Id == items.ProductId).FirstOrDefault();
                    selected_product.Stock = selected_product.Stock - items.Quantity;
                    _dbContext.Products.Update(selected_product);
                }
                _dbContext.SaveChanges();

                ResponseModel response = new ResponseModel();
                response.Message = OrderId;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Siparis numarasi (OrderId) uretme islemi
        private string GenerateOrderId()
        {
            string OrderId = string.Empty;
            Random generator = null;
            for (int i = 0; i < 1000; i++)
            {
                generator = new Random();
                OrderId = "p" + generator.Next(1, 10000000).ToString("D8");
                if (!_dbContext.CustomerOrders.Where(x => x.OrderId == OrderId).Any()) // Uretilen siparis no (OrderId) ile db'de eslesen kayit yoksa donguden cik. (Eger eslesen kayit varsa yeni bir numara uretilecek.)
                {
                    break;
                }
            }
            return OrderId;
        }

        // Giris yapan kisinin vermis oldugu tum siparisler Id'ye gore azalan bicimde listeleniyor.
        public List<CustomerOrder> GetOrdersByCustomerId(int customerId)
        {
            var _customerOrders = _dbContext.CustomerOrders.Where(x => x.CustomerId == customerId).OrderByDescending(x => x.Id).ToList();
            return _customerOrders;
        }

        public List<CartModel> GetOrderDetailForCustomer(int customerId, string order_number)
        {
            List<CartModel> cart_details = new List<CartModel>();

            // Giren kisinin id'si ve siparisin id'si esit olan tum musteri siparislerini listele
            var customer_order = _dbContext.CustomerOrders.Where(x => x.CustomerId == customerId && x.OrderId == order_number).FirstOrDefault();

            if (customer_order != null)
            {
                var order_detail = _dbContext.OrderDetails.Where(x => x.OrderId == order_number).ToList();
                var product_list = _dbContext.Products.ToList(); // Tum urun listesi

                foreach (var order in order_detail)
                {
                    // tum urunler arasindan id'si siparisteki ProductId'ye esit olani getir
                    var prod = product_list.Where(x => x.Id == order.ProductId).FirstOrDefault();
                    CartModel _cartModel = new CartModel();
                    _cartModel.ProductName = prod.Name;
                    _cartModel.Price = Convert.ToInt32(order.Price);
                    _cartModel.ProductImage = prod.ImageUrl;
                    _cartModel.Quantity = Convert.ToInt32(order.Quantity);
                    cart_details.Add(_cartModel);
                }

                cart_details.FirstOrDefault().ShippingAddress = customer_order.ShippingAddress; // musterinin siparisindeki siparis adresini cart_details'teki ilk ogesine atadik.
                cart_details.FirstOrDefault().ShippingCharges = Convert.ToInt32(customer_order.ShippingCharges);
                cart_details.FirstOrDefault().SubTotal = Convert.ToInt32(customer_order.SubTotal);
                cart_details.FirstOrDefault().Total = Convert.ToInt32(customer_order.Total);
                cart_details.FirstOrDefault().PaymentMode = customer_order.PaymentMode;
            }
            return cart_details;
        }

        public ResponseModel ChangePassword(PasswordModel passwordModel)
        {
            ResponseModel response = new ResponseModel();
            response.Status = true;

            var _customer = _dbContext.Customers.Where(x => x.Id == passwordModel.UserKey).FirstOrDefault(); // Giris yapan kisinin key bilgisine esit olar Customer'i cektik.
            if (_customer != null)
            {
                _customer.Password = passwordModel.Password;
                _dbContext.Customers.Update(_customer);
                _dbContext.SaveChanges();

                response.Message = "Password updated successfully.";
            }
            else
            {
                response.Message = "User does not exist. Try again!";
            }
            return response;
        }

        // Verilen siparis numarasina gore siparisin gonderim durumunu liste olarak dondurur.
        public List<string> GetShippingStatusForOrder(string order_number)
        {
            List<string> shipping_status = new List<string>();
            var order = _dbContext.CustomerOrders.Where(x => x.OrderId == order_number).FirstOrDefault();
            if (order != null && order.ShippingStatus != null)
            {
                shipping_status = order.ShippingStatus.Split("|").ToList();
            }
            return shipping_status;
        }

        // Kartla odeme yontemi (Kredi karti bilgislerini dogrusan apiye gondermek guvenli olmadigi icin tam calismiyor)
        public async Task<string> MakePaymentStripe(string cardNumber, int expMonth, int expYear, string cvc, decimal value)
        {
            // http://localhost:5265/api/User/CheckoutStripe/?cardNumber=4242424242424242&expMonth=05&expYear=24&cvc=123&value=5
            try
            {
                // Asagidaki satirda yer alan ApiKey https://dashboard.stripe.com/apikeys sitesinden olusturulmustur.
                StripeConfiguration.ApiKey = "...";
                var optionToken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = cardNumber,
                        ExpMonth = expMonth.ToString(),
                        ExpYear = expYear.ToString(),
                        Cvc = cvc
                    }
                };

                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(optionToken);

                var customer = new Stripe.Customer
                {
                    Name = "Büşra",
                    Address = new Address
                    {
                        Country = "Turkey",
                        City = "Duzce",
                        Line1 = "59 - Center, Duzce",
                        PostalCode = "81100",
                    }
                };

                var options = new ChargeCreateOptions()
                {
                    Amount = (long)(value * 100), // Kuruş cinsinden
                    Currency = "usd", // "ABD" yerine "usd" kullanın
                    Description = "Test",
                    Source = stripeToken.Id,

                };

                var service = new ChargeService();
                Charge charge=await service.CreateAsync(options);

                if (charge.Paid)
                {
                    return "Success";
                }
                else
                {
                    return "Fail";
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
