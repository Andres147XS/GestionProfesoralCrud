using System.Net.Http.Json;

namespace GestionProfesoral.Web.Services
{
    // Interfaz del CRUD 
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

        public CrudService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<T>?> GetAllAsync(string endpoint)
        {
            // GET 
            return await _httpClient.GetFromJsonAsync<List<T>>($"api/{endpoint}");
        }

        public async Task<T?> GetByIdAsync(string endpoint, object id)
        {
            // GET {id}
            return await _httpClient.GetFromJsonAsync<T>($"api/{endpoint}/{id}");
        }

        public async Task<bool> CreateAsync(string endpoint, T item)
        {
            // POST 
            var response = await _httpClient.PostAsJsonAsync($"api/{endpoint}", item);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(string endpoint, object id, T item)
        {
            // PUT {id}
            var response = await _httpClient.PutAsJsonAsync($"api/{endpoint}/{id}", item);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint, object id)
        {
            // DELETE {id}
            var response = await _httpClient.DeleteAsync($"api/{endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
