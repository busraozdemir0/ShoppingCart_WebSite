using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Shop.DataModels.CustomModels;
using Shop.DataModels.Models;
using System.Text.Json;

namespace Shop.Web.Services
{
    public class UserPanelService : IUserPanelService
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedSessionStorage _sessionStorage;

        public UserPanelService(HttpClient httpClient, ProtectedSessionStorage sessionStorage)
        {
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
        }

        public async Task<bool> IsUserLoggedIn()
        {
            bool flag = false;
            var result = await _sessionStorage.GetAsync<string>("userKey");
            if (result.Success)
            {
                flag = true;
            }
            return flag;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryModel>>("api/User/GetCategories");
        }
        public async Task<List<ProductModel>> GetProductByCategoryId(int categoryId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ProductModel>>("api/User/GetProductByCategoryId/?categoryId=" + categoryId);
            }
            catch (Exception ex)
            {
                // Hata mesajını loglayın veya konsolda yazdırın
                Console.WriteLine($"Hata: {ex.Message}");
                return new List<ProductModel>();
            }
        }

        public async Task<ResponseModel> RegisterUser(RegisterModel registerModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/User/RegisterUser", registerModel);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseModel>();
        }

        public async Task<ResponseModel> LoginUser(LoginModel loginModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/User/LoginUser", loginModel);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseModel>();
        }

        // Siparisi verme islemi
        public async Task<ResponseModel> Checkout(List<CartModel> cartItems)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/User/Checkout", cartItems);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseModel>();
        }

        // Giren kullanicinin id'sine gore siparisleri getir
        public async Task<List<CustomerOrder>> GetOrdersByCustomerId(int customerId)
        {
            return await _httpClient.GetFromJsonAsync<List<CustomerOrder>>("api/User/GetOrdersByCustomerId/?customerId=" + customerId);
        }

        public async Task<List<CartModel>> GetOrderDetailForCustomer(int customerId, string order_number)
        {
            return await _httpClient.GetFromJsonAsync<List<CartModel>>("api/User/GetOrderDetailForCustomer/?customerId=" + customerId + "&order_number=" + order_number);
        }
        public async Task<List<string>> GetShippingStatusForOrder(string order_number)
        {
            return await _httpClient.GetFromJsonAsync<List<string>>("api/User/GetShippingStatusForOrder/?order_number=" + order_number);
        }

        public async Task<ResponseModel> ChangePassword(PasswordModel passwordModel)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/User/ChangePassword", passwordModel);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseModel>();
        }
    }
}
