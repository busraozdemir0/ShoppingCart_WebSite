using Shop.DataModels.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Services
{
    public interface IAdminService
    {
        ResponseModel AdminLogin(LoginModel loginModel);
        CategoryModel SaveCategory(CategoryModel newCategory);
        List<CategoryModel> GetCategories();
        bool UpdateCategory(CategoryModel categoryToUpdate);
        bool DeleteCategory(CategoryModel categoryToDelete);


        List<ProductModel> GetProducts();
        bool DeleteProduct(ProductModel productToDelete);
        int GetNewProductId();
        ProductModel SaveProduct(ProductModel newProduct);

        List<StockModel> GetProductStock();
        bool UpdateProductStock(StockModel stock);
        List<OrdersModel> GetOrderDetail(); // Verilmis tum siparisler
        CustomerOrderDetailModel GetCustomerOrderDetailByOrderId(string orderId); // Verilen siparisin kimin verdigi vs bilgileri gormek icin orderId'ye gore siparis bilgilerini getirme
    }
}
