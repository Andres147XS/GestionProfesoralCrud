using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace GestionProfesoral.Web.Services
{
    public interface ICrudService<T> where T : class
    {
        Task<List<T>?> GetAllAsync(string endpoint);
        Task<T?> GetByIdAsync(string endpoint, object id);
        Task<bool> CreateAsync(string endpoint, T item);
        Task<bool> UpdateAsync(string endpoint, object id, T item);
        Task<bool> DeleteAsync(string endpoint, object id);
    }

    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CrudService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task AgregarTokenAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsStringAsync(AuthService.TokenKey);
                if (!string.IsNullOrWhiteSpace(token))
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
            }
            catch { /* localStorage puede no estar disponible en prerender */ }
        }

        public async Task<List<T>?> GetAllAsync(string endpoint)
        {
<<<<<<< Updated upstream
            // GET 
            return await _httpClient.GetFromJsonAsync<List<T>>($"api/{endpoint}");
=======
            try
            {
                await AgregarTokenAsync();
                return await _httpClient.GetFromJsonAsync<List<T>>($"api/{endpoint}");
            }
            catch { return null; }
>>>>>>> Stashed changes
        }

        public async Task<T?> GetByIdAsync(string endpoint, object id)
        {
<<<<<<< Updated upstream
            // GET {id}
            return await _httpClient.GetFromJsonAsync<T>($"api/{endpoint}/{id}");
=======
            try
            {
                await AgregarTokenAsync();
                return await _httpClient.GetFromJsonAsync<T>($"api/{endpoint}/{id}");
            }
            catch { return null; }
>>>>>>> Stashed changes
        }

        public async Task<bool> CreateAsync(string endpoint, T item)
        {
<<<<<<< Updated upstream
            // POST 
            var response = await _httpClient.PostAsJsonAsync($"api/{endpoint}", item);
            return response.IsSuccessStatusCode;
=======
            try
            {
                await AgregarTokenAsync();
                var response = await _httpClient.PostAsJsonAsync($"api/{endpoint}", item);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
>>>>>>> Stashed changes
        }

        public async Task<bool> UpdateAsync(string endpoint, object id, T item)
        {
<<<<<<< Updated upstream
            // PUT {id}
            var response = await _httpClient.PutAsJsonAsync($"api/{endpoint}/{id}", item);
            return response.IsSuccessStatusCode;
=======
            try
            {
                await AgregarTokenAsync();
                var response = await _httpClient.PutAsJsonAsync($"api/{endpoint}/{id}", item);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
>>>>>>> Stashed changes
        }

        public async Task<bool> DeleteAsync(string endpoint, object id)
        {
<<<<<<< Updated upstream
            // DELETE {id}
            var response = await _httpClient.DeleteAsync($"api/{endpoint}/{id}");
            return response.IsSuccessStatusCode;
=======
            try
            {
                await AgregarTokenAsync();
                var response = await _httpClient.DeleteAsync($"api/{endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
>>>>>>> Stashed changes
        }
    }
}
