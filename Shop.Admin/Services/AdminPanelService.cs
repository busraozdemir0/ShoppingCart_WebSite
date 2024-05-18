using Shop.DataModels.CustomModels;
using System.Net.Http;
using System.Net.Http.Json;

namespace Shop.Admin.Services
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly HttpClient _httpClient;

        public AdminPanelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel> AdminLogin(LoginModel loginModel)
        {
          //  return await _httpClient.PostAsJsonAsync<ResponseModel>("api/admin/AdminLogin", loginModel);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/AdminLogin", loginModel);

            // Yanıtın başarılı olup olmadığını kontrol edin
            if (!response.IsSuccessStatusCode)
            {
                // Yanıtın detaylarını al
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            // Yanıtı deserialize edin
            return await response.Content.ReadFromJsonAsync<ResponseModel>();
        }

    }
}
