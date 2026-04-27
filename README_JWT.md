# Tercera Entrega — JWT + Control de Acceso por Roles

## Credenciales de prueba

| Usuario | Contraseña | Rol           |
|---------|------------|---------------|
| admin   | Admin123!  | Administrador |

---

## Archivos nuevos y modificados

### GestionProfesoral.Shared
- `Models/Rol.cs` — Modelo de Rol
- `Models/Usuario.cs` — Modelo de Usuario (con PasswordHash BCrypt)
- `DTOs/AuthDtos.cs` — DTOs para login, crear/editar usuario

### GestionProfesoral.API
- `appsettings.json` — Sección `Jwt` (clave, issuer, audience, expiración)
- `GestionProfesoral.API.csproj` — Paquetes: BCrypt.Net-Next, JwtBearer
- `Program.cs` — `UseAuthentication()`, `AddAuthentication(JwtBearer)`, `AddAuthorization()`
- `Data/AppDbContext.cs` — DbSets Roles/Usuarios + seed data + relaciones
- `Controllers/AuthController.cs` — **NUEVO**: POST /api/auth/login → devuelve JWT
- `Controllers/UsuarioController.cs` — **NUEVO**: CRUD, `[Authorize(Roles="Administrador")]`
- `Controllers/RolController.cs` — **NUEVO**: CRUD, `[Authorize(Roles="Administrador")]`
- `Controllers/*.cs` (todos los existentes) — Se les agregó `[Authorize]`
- `Migrations/20260416000000_AddAuthTables.cs` — **NUEVO**: crea tablas Roles y Usuarios

### GestionProfesoral.Web
- `GestionProfesoral.Web.csproj` — Paquetes: Blazored.LocalStorage, Components.Authorization, System.IdentityModel.Tokens.Jwt
- `Program.cs` — `AddBlazoredLocalStorage`, `AddAuthorizationCore`, `JwtAuthStateProvider`
- `App.razor` — `<CascadingAuthenticationState>` + `<AuthorizeRouteView>`
- `_Imports.razor` — Agrega `@using Microsoft.AspNetCore.Components.Authorization`
- `Services/AuthService.cs` — **NUEVO**: login/logout, guarda token en localStorage
- `Services/JwtAuthStateProvider.cs` — **NUEVO**: lee JWT, valida expiración, expone claims
- `Services/CrudService.cs` — Modificado: agrega header `Authorization: Bearer {token}`
- `Components/RedirectToLogin.razor` — **NUEVO**: redirige a /login si no autenticado
- `Layout/MainLayout.razor` — Muestra usuario/rol en barra superior + botón Salir
- `Layout/NavMenu.razor` — Menú filtrado por rol (`<AuthorizeView Roles="Administrador">`)
- `Pages/Login.razor` — **NUEVO**: formulario de login
- `Pages/Home.razor` — Bienvenida personalizada según usuario/rol
- `Pages/Usuarios/Index.razor` — **NUEVO**: listado de usuarios (solo Admin)
- `Pages/Usuarios/Crear.razor` — **NUEVO**: crear usuario (solo Admin)
- `Pages/Usuarios/Editar.razor` — **NUEVO**: editar usuario (solo Admin)
- `Pages/Roles/Index.razor` — **NUEVO**: listado de roles (solo Admin)
- `Pages/Roles/Crear.razor` — **NUEVO**: crear rol (solo Admin)
- `Pages/Roles/Editar.razor` — **NUEVO**: editar rol (solo Admin)
- `Pages/**/*.razor` (todos los existentes) — Se les agregó `@attribute [Authorize]`

---

## Flujo de seguridad

```
Usuario → /login → POST /api/auth/login
         ← JWT (HS256, 8h, claims: nombre, correo, rol)
         → localStorage["authToken"]
         → JwtAuthStateProvider notifica cambio
         → NavMenu muestra menú según rol
         → CrudService agrega header Authorization en cada llamada
         → API valida JWT en cada endpoint
         → [Authorize(Roles="Administrador")] bloquea acceso a Usuarios/Roles
```

---

## Pasos para ejecutar

1. **Ejecutar la API** (`GestionProfesoral.API`):
   ```
   dotnet run --project GestionProfesoral.API
   ```
   La base de datos se crea automáticamente con `EnsureCreated()`.
   Las tablas Roles y Usuarios se insertan con seed data.

2. **Ejecutar el Frontend** (`GestionProfesoral.Web`):
   ```
   dotnet run --project GestionProfesoral.Web
   ```

3. Abrir el navegador en `https://localhost:PORT`

4. Iniciar sesión con `admin` / `Admin123!`

5. Como administrador verás en el menú lateral la sección **ADMINISTRACIÓN** con acceso a Usuarios y Roles.

---

## Seguridad implementada

| Capa        | Mecanismo                                                              |
|-------------|------------------------------------------------------------------------|
| API         | JWT Bearer Authentication (HS256, firmado con clave secreta)           |
| API         | `[Authorize]` en todos los controllers existentes                      |
| API         | `[Authorize(Roles="Administrador")]` en UsuarioController y RolController |
| API         | BCrypt para hash de contraseñas (nunca texto plano)                    |
| Frontend    | `AuthorizeRouteView` redirige a /login si no autenticado               |
| Frontend    | `@attribute [Authorize]` en todas las páginas existentes               |
| Frontend    | `@attribute [Authorize(Roles="Administrador")]` en páginas de Admin    |
| Frontend    | `<AuthorizeView>` oculta ítems del menú según rol                      |
| Frontend    | JWT se valida localmente (expiración) antes de cada navegación         |
