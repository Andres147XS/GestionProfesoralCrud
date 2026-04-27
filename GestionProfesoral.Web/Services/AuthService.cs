using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using GestionProfesoral.Shared.DTOs;

namespace GestionProfesoral.Web.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<(bool Exitoso, string Mensaje)> RegistroAsync(RegistroRequestDto request);
        Task LogoutAsync();
        Task<string?> GetTokenAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        public const string TokenKey = "authToken";

        public AuthService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", request);
                if (!response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (result != null)
                    await _localStorage.SetItemAsStringAsync(TokenKey, result.Token);

                return result;
            }
            catch { return null; }
        }

        public async Task<(bool Exitoso, string Mensaje)> RegistroAsync(RegistroRequestDto request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/registro", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                    if (result != null)
                        await _localStorage.SetItemAsStringAsync(TokenKey, result.Token);
                    return (true, string.Empty);
                }

                // Leer mensaje de error del servidor
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var error = JsonSerializer.Deserialize<ErrorResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (false, error?.Mensaje ?? "No se pudo completar el registro");
                }
                catch
                {
                    return (false, "No se pudo completar el registro");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }

        public async Task<string?> GetTokenAsync()
        {
            try { return await _localStorage.GetItemAsStringAsync(TokenKey); }
            catch { return null; }
        }

        private class ErrorResponse
        {
            public string Mensaje { get; set; } = string.Empty;
        }
    }
}
