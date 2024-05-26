using Shop.DataModels.CustomModels;

namespace Shop.Admin.Services
{
    public interface IAdminPanelService
    {
        Task<ResponseModel> AdminLogin(LoginModel loginModel);
        Task<CategoryModel> SaveCategory(CategoryModel newCategory);
        Task<List<CategoryModel>> GetCategories();
        Task<List<ProductModel>> GetProducts();
        Task<bool> UpdateCategory(CategoryModel categoryToUpdate);
        Task<bool> DeleteCategory(CategoryModel categoryToDelete);
        Task<bool> DeleteProduct(ProductModel productToDelete);
        Task<ProductModel> SaveProduct(ProductModel newProduct);
        Task<List<StockModel>> GetProductStock();
        Task<bool> UpdateProductStock(StockModel stock);
        Task<List<OrdersModel>> GetOrderDetail(); // Tum siparisleri listeleme
        Task<CustomerOrderDetailModel> GetCustomerOrderDetailByOrderId(string orderId); // Gelen orderID'ye gore CustomerOrder tablosundaki musteri bilgilerini getirme
    }
}
