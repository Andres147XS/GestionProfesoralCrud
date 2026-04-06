using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datosSQL (Server usando la cadena "DefaultConnection")
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS: permite que el frontend pueda consumir la API sin bloqueo del navegador
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Crear la base de datos y las tablas si no existen
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // ATENCIÓN: La Opción 1 consiste en descomentar estas dos líneas para borrar y recrear la base de datos con todas las tablas.
    // context.Database.EnsureCreated();

    // Re-crear tablas para aplicar cambios de ValueGeneratedNever

    //context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    // Crear Procedimiento Almacenado para insertar docentes si no existe
    var sql = @"
        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertarDocente]') AND type in (N'P', N'PC'))
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
        END";
    context.Database.ExecuteSqlRaw(sql);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

// Activa las rutas de los controladores: /api/Red, /api/Beca, /api/ApoyoProfesoral, /api/Aliado
app.MapControllers();

app.Run();
