using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using GestionProfesoral.Web;
using GestionProfesoral.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7019/")
});

// LocalStorage (Blazored)
builder.Services.AddBlazoredLocalStorage();

// Autenticacion y autorizacion
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

// CRUD generico
builder.Services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));

await builder.Build().RunAsync();
