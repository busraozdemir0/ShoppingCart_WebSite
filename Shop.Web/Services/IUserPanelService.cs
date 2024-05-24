using Shop.DataModels.CustomModels;

namespace Shop.Web.Services
{
    public interface IUserPanelService
    {
        Task<bool> IsUserLoggedIn();
        Task<List<CategoryModel>> GetCategories();
        Task<List<ProductModel>> GetProductByCategoryId(int categoryId);
        Task<ResponseModel> RegisterUser(RegisterModel registerModel);
        Task<ResponseModel> LoginUser(LoginModel loginModel);
    }
}
