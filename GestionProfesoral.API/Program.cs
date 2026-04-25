using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using GestionProfesoral.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionProfesoral API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        Array.Empty<string>()
    }});
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

    context.Database.ExecuteSqlRaw(@"
        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Roles')
        BEGIN
            CREATE TABLE [dbo].[Roles] (
                [Id]          INT            IDENTITY(1,1) NOT NULL PRIMARY KEY,
                [Nombre]      NVARCHAR(50)   NOT NULL,
                [Descripcion] NVARCHAR(200)  NULL
            )
        END
    ");

    context.Database.ExecuteSqlRaw(@"
        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Usuarios')
        BEGIN
            CREATE TABLE [dbo].[Usuarios] (
                [Id]            INT            IDENTITY(1,1) NOT NULL PRIMARY KEY,
                [NombreUsuario] NVARCHAR(60)   NOT NULL,
                [PasswordHash]  NVARCHAR(256)  NOT NULL,
                [Correo]        NVARCHAR(100)  NOT NULL,
                [RolId]         INT            NOT NULL,
                [Activo]        BIT            NOT NULL DEFAULT(1),
                CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY ([RolId])
                    REFERENCES [dbo].[Roles] ([Id])
            )
            CREATE UNIQUE INDEX [IX_Usuarios_NombreUsuario]
                ON [dbo].[Usuarios] ([NombreUsuario])
        END
    ");

    context.Database.ExecuteSqlRaw(@"
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [Nombre] = 'Administrador')
            INSERT INTO [dbo].[Roles] ([Nombre],[Descripcion])
            VALUES ('Administrador','Acceso total al sistema incluyendo CRUD de usuarios y roles')

        IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [Nombre] = 'Docente')
            INSERT INTO [dbo].[Roles] ([Nombre],[Descripcion])
            VALUES ('Docente','Acceso a modulos de gestion profesoral')

        IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [Nombre] = 'Consultor')
            INSERT INTO [dbo].[Roles] ([Nombre],[Descripcion])
            VALUES ('Consultor','Solo lectura de informacion')
    ");

    context.Database.ExecuteSqlRaw(@"
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Usuarios] WHERE [NombreUsuario] = 'admin')
        BEGIN
            DECLARE @rolAdminId INT
            SELECT @rolAdminId = [Id] FROM [dbo].[Roles] WHERE [Nombre] = 'Administrador'
            INSERT INTO [dbo].[Usuarios] ([NombreUsuario],[PasswordHash],[Correo],[RolId],[Activo])
            VALUES (
                'admin',
                '$2b$10$/pEsxDsbMoUzQsc9FSC9l.S0DqMgRBH81tArmW76arHE9GovZtP1q',
                'admin@gestionprofesoral.edu.co',
                @rolAdminId,
                1
            )
        END
    ");

    context.Database.ExecuteSqlRaw(@"
        IF NOT EXISTS (
            SELECT * FROM sys.objects
            WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarDocente]')
              AND type IN (N'P', N'PC')
        )
        BEGIN
            EXEC('CREATE PROCEDURE [dbo].[sp_InsertarDocente]
                @Cedula int,
                @Nombres nvarchar(60),
                @Apellidos nvarchar(60),
                @Correo nvarchar(70),
                @Cargo nvarchar(30)
            AS
            BEGIN
                INSERT INTO Docentes (Cedula, Nombres, Apellidos, Correo, Cargo)
                VALUES (@Cedula, @Nombres, @Apellidos, @Correo, @Cargo)
            END')
        END
    ");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
