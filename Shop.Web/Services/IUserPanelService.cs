using Shop.DataModels.CustomModels;

namespace Shop.Web.Services
{
    public interface IUserPanelService
    {
        Task<List<CategoryModel>> GetCategories();
        Task<List<ProductModel>> GetProductByCategoryId(int categoryId);
    }
}
