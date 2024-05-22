using Shop.DataModels.CustomModels;

namespace Shop.Web.Services
{
    public class UserPanelService : IUserPanelService
    {
        private readonly HttpClient _httpClient;

        public UserPanelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryModel>>("api/User/GetCategories");
        }

        public async Task<List<ProductModel>> GetProductByCategoryId(int categoryId)
        {
            return await _httpClient.GetFromJsonAsync<List<ProductModel>>("api/User/GetProductByCategoryId/?categoryId=" + categoryId);

        }
    }
}
