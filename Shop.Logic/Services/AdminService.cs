using Shop.DataModels.CustomModels;
using Shop.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ShoppingCartDBContext _dbContext = null;

        public AdminService(ShoppingCartDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseModel AdminLogin(LoginModel loginModel)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                var userData = _dbContext.AdminInfos.Where(x => x.Email == loginModel.EmailId).FirstOrDefault();
                if (userData is not null)
                {
                    if (userData.Password == loginModel.Password)
                    {
                        responseModel.Status = true;
                        responseModel.Message = Convert.ToString(userData.Id) + "|" + userData.Name + "|" + userData.Email;
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = "Your Password is Incorrect";
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Email not registered. Please check your Email Id";
                }
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = "An Error has occured. Please try again!";

                return responseModel;
            }

        }

        public CategoryModel SaveCategory(CategoryModel newCategory)
        {
            try
            {
                Category _category = new Category();
                _category.Name = newCategory.Name;
                _dbContext.Add(_category);
                _dbContext.SaveChanges();
                return newCategory;
            }
            catch (Exception ex)
            {
                throw;
            }

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

        public bool UpdateCategory(CategoryModel categoryToUpdate)
        {
            bool flag = false;
            var _category = _dbContext.Categories.Where(x => x.Id == categoryToUpdate.Id).First();
            if (_category is not null)
            {
                _category.Name = categoryToUpdate.Name;
                _dbContext.Categories.Update(_category);
                _dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public bool DeleteCategory(CategoryModel categoryToDelete)
        {
            bool flag = false;
            var _category = _dbContext.Categories.Where(x => x.Id == categoryToDelete.Id).First();
            if (_category is not null)
            {
                _dbContext.Categories.Remove(_category);
                _dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }


        public List<ProductModel> GetProducts()
        {
            List<Category> categoryData = _dbContext.Categories.ToList();
            List<Product> productData = _dbContext.Products.ToList();
            List<ProductModel> _productList = new List<ProductModel>();

            foreach (var p in productData)
            {
                ProductModel _productModel = new ProductModel();
                _productModel.Id = p.Id;
                _productModel.Name = p.Name;
                _productModel.Price = p.Price;
                _productModel.Stock = p.Stock;
                _productModel.ImageUrl = p.ImageUrl;
                _productModel.CategoryId = p.CategoryId;
                _productModel.CategoryName = categoryData.Where(x => x.Id == p.CategoryId).Select(x => x.Name).FirstOrDefault();
                _productList.Add(_productModel);
            }
            return _productList;
        }

        public bool DeleteProduct(ProductModel productToDelete)
        {
            bool flag = false;
            var _product = _dbContext.Products.Where(x => x.Id == productToDelete.Id).First();
            if (_product is not null)
            {
                _dbContext.Products.Remove(_product);
                _dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public int GetNewProductId()
        {
            try
            {
                int nextProductId = _dbContext.Products.ToList().OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                return nextProductId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ProductModel SaveProduct(ProductModel newProduct)
        {
            try
            {
                Product _product = new Product();
                _product.Name = newProduct.Name;
                _product.Price = newProduct.Price;
                _product.Stock = newProduct.Stock;
                _product.ImageUrl = newProduct.ImageUrl;
                _product.CategoryId = newProduct.CategoryId;
                _dbContext.Add(_product);
                _dbContext.SaveChanges();
                return newProduct;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<StockModel> GetProductStock()
        {
            List<StockModel> productStock = new List<StockModel>();

            List<Category> categoryData = _dbContext.Categories.ToList();
            List<Product> productData = _dbContext.Products.ToList();

            foreach (var p in productData)
            {
                // Mevcutta var olan urunun adini, stogunu, kategorisini StockModel'e atama (ardindan bu Stock Model uzerinden stok guncelleme islemi yapilacak)
                StockModel _stockModel = new StockModel();
                _stockModel.Id = p.Id;
                _stockModel.Name = p.Name;
                _stockModel.Stock = p.Stock;
                _stockModel.CategoryName = categoryData.Where(x => x.Id == p.CategoryId).Select(x => x.Name).FirstOrDefault();
                productStock.Add(_stockModel);

            }
            return productStock;
        }

        // Urunun stogunu guncelleme islemi
        public bool UpdateProductStock(StockModel stock)
        {
            bool flag = false;
            var _product = _dbContext.Products.Where(x => x.Id == stock.Id).First(); // Gelen stock id'sini Product tablosunda bul
            if (_product != null) // product bos donmuyorsa stock guncelleme islemi
            {
                _product.Stock = stock.Stock + stock.NewStock; // Mevcutta var olan stok miktari uzerine yeni stok miktari ekleme
                _dbContext.Products.Update(_product);
                _dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public List<OrdersModel> GetOrderDetail()
        {
            List<OrdersModel> orderModelList = new List<OrdersModel>();
            List<Product> productData = _dbContext.Products.ToList();

            List<OrderDetail> orderDetailData = _dbContext.OrderDetails.ToList();

            foreach (var o in orderDetailData)
            {
                OrdersModel _ordersModel = new OrdersModel();
                _ordersModel.Id = o.Id;
                _ordersModel.OrderId = o.OrderId;
                _ordersModel.ProductId = o.ProductId;
                _ordersModel.ProductName = productData.Where(x => x.Id == o.ProductId).Select(x => x.Name).FirstOrDefault();
                _ordersModel.Quantity = o.Quantity;
                _ordersModel.Price = o.Price;
                _ordersModel.SubTotal = o.SubTotal;
                _ordersModel.CreatedOn = o.CreatedOn;
                orderModelList.Add(_ordersModel);

            }
            return orderModelList;
        }

        public CustomerOrderDetailModel GetCustomerOrderDetailByOrderId(string orderId)
        {
            List<Customer> customers = _dbContext.Customers.ToList(); // tum musteriler

            var customerOrderDetail = _dbContext.CustomerOrders.Where(x => x.OrderId == orderId).FirstOrDefault();

            CustomerOrderDetailModel model = new CustomerOrderDetailModel();
            model.OrderId = customerOrderDetail.OrderId;
            model.CustomerId = customerOrderDetail.CustomerId;
            model.CustomerNameSurname = customers.Where(x => x.Id == model.CustomerId).Select(x => x.Name).FirstOrDefault(); // Modeldeki musteri id'ye esit olani musteriler tablosundan bul ve adini cek.
            model.CustomerEmail = customers.Where(x => x.Id == model.CustomerId).Select(x => x.Email).FirstOrDefault(); // Musterinin e-mail bilgisi
            model.CustomerMobileNo = customers.Where(x => x.Id == model.CustomerId).Select(x => x.MobileNo).FirstOrDefault(); // Musterinin telefon bilgisi
            model.Total=customerOrderDetail.Total;
            model.SubTotal=customerOrderDetail.SubTotal;
            model.PaymentMode=customerOrderDetail.PaymentMode;
            model.ShippingAddress=customerOrderDetail.ShippingAddress;
            model.ShippingCharges=customerOrderDetail.ShippingCharges;
            model.ShippingStatus=customerOrderDetail.ShippingStatus;

            return model;
        }
    }
}
