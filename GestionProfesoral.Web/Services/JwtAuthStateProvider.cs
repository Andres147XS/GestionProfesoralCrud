using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace GestionProfesoral.Web.Services
{
    /// <summary>
    /// Lee el JWT del localStorage, valida expiración y expone claims de usuario
    /// (nombre, rol) a toda la app Blazor via CascadingAuthenticationState.
    /// </summary>
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private static readonly AuthenticationState Anonimo =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        // Mapeo de nombres cortos JWT → ClaimTypes estándar de .NET
        private static readonly Dictionary<string, string> ClaimMap = new()
        {
            ["nameid"]  = ClaimTypes.NameIdentifier,
            ["unique_name"] = ClaimTypes.Name,
            ["email"]   = ClaimTypes.Email,
            ["role"]    = ClaimTypes.Role,
        };

        public JwtAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token;
            try { token = await _localStorage.GetItemAsStringAsync(AuthService.TokenKey); }
            catch { return Anonimo; }

            if (string.IsNullOrWhiteSpace(token))
                return Anonimo;

            // Leer y validar token
            JwtSecurityToken jwt;
            try
            {
                // Desactivar el mapeo automático de tipos (que cambia "role" por URI larga)
                var handler = new JwtSecurityTokenHandler();
                handler.InboundClaimTypeMap.Clear();
                jwt = handler.ReadJwtToken(token);
            }
            catch { return Anonimo; }

            // Verificar expiración
            if (jwt.ValidTo < DateTime.UtcNow)
            {
                await _localStorage.RemoveItemAsync(AuthService.TokenKey);
                return Anonimo;
            }

            // Remapear claims JWT a ClaimTypes estándar de .NET
            var claims = jwt.Claims.Select(c =>
            {
                var tipo = ClaimMap.TryGetValue(c.Type, out var mapped) ? mapped : c.Type;
                return new Claim(tipo, c.Value);
            }).ToList();

            // Configurar header Authorization para HttpClient
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Crear identidad indicando explícitamente qué claim es el rol
            var identity = new ClaimsIdentity(claims, "jwt",
                nameType: ClaimTypes.Name,
                roleType: ClaimTypes.Role);

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        /// <summary>Notifica a todos los componentes que el estado de autenticación cambió.</summary>
        public void NotificarCambio()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
