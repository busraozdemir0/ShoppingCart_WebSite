using Shop.DataModels.CustomModels;
using System.Net.Http;
using System.Net.Http.Json;

namespace Shop.Admin.Services
{
    // Burada yer alan metotlar verilen nesneyi API endpointine gondererek ilgili adimlari isler.
    // Yani projede yer alan API katmani burada kullanilmaktadir.
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

            // Yanitin basarili olup olmadigini kontrol edin
            if (!response.IsSuccessStatusCode)
            {
                // Yanitin detaylarini al
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            // Yanıtı deserialize edin
            return await response.Content.ReadFromJsonAsync<ResponseModel>();
        }

        public async Task<CategoryModel> SaveCategory(CategoryModel newCategory)
        {

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/SaveCategory", newCategory);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<CategoryModel>();
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryModel>>("api/Admin/GetCategories");
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductModel>>("api/Admin/GetProducts");
        }

        public async Task<bool> UpdateCategory(CategoryModel categoryToUpdate)
        {
          //  return await _httpClient.PostAsJsonAsync<bool>("api/Admin/UpdateCategory",categoryToUpdate);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/UpdateCategory", categoryToUpdate);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> DeleteCategory(CategoryModel categoryToDelete)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/DeleteCategory", categoryToDelete);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> DeleteProduct(ProductModel productToDelete)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/DeleteProduct", productToDelete);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<ProductModel> SaveProduct(ProductModel newProduct)
        {

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/SaveProduct", newProduct);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<ProductModel>();
        }

        public async Task<List<StockModel>> GetProductStock()
        {
            return await _httpClient.GetFromJsonAsync<List<StockModel>>("api/Admin/GetProductStock");
        }

        // Bu metot verilen  StockModel nesnesini bir API endpointine gonderir ve islemin basarili olup olmadigini kontrol eder. Eger istek basarili olursa yanit icerigi JSON formatinda okunur ve bool turune donusturulur.
        public async Task<bool> UpdateProductStock(StockModel stock)  // Urunun stogunu guncelleme islemi
        {

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/UpdateProductStock", stock);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<bool>(); // Yaniti bool turune donusturme islemi
        }

        public async Task<List<OrdersModel>> GetOrderDetail()
        {
            return await _httpClient.GetFromJsonAsync<List<OrdersModel>>("api/Admin/GetOrderDetail");
        }

        public async Task<CustomerOrderDetailModel> GetCustomerOrderDetailByOrderId(string orderId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CustomerOrderDetailModel>("api/Admin/GetCustomerOrderDetailByOrderId/?orderId=" + orderId);

            }
            catch (Exception ex)
            {
                // Hata mesajını loglayın veya konsolda yazdırın
                Console.WriteLine($"Hata: {ex.Message}");
                return new CustomerOrderDetailModel();
            }
        }

        public async Task<bool> UpdateOrderShippingStatus(CustomerOrderDetailModel order)  // Siparisin durumunu guncelleme islemi
        {

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/UpdateOrderShippingStatus", order);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Status: {response.StatusCode}, Content: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<bool>(); // Yaniti bool turune donusturme islemi
        }

    }
}
