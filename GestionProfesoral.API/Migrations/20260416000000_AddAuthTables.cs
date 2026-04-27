using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionProfesoral.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            // Seed roles
            migrationBuilder.InsertData("Roles", new[] { "Id", "Nombre", "Descripcion" }, new object[] { 1, "Administrador", "Acceso total al sistema incluyendo CRUD de usuarios y roles" });
            migrationBuilder.InsertData("Roles", new[] { "Id", "Nombre", "Descripcion" }, new object[] { 2, "Docente", "Acceso a modulos de gestion profesoral" });
            migrationBuilder.InsertData("Roles", new[] { "Id", "Nombre", "Descripcion" }, new object[] { 3, "Consultor", "Solo lectura de informacion" });

            // Seed admin user - password: Admin123!
            migrationBuilder.InsertData("Usuarios",
                new[] { "Id", "NombreUsuario", "PasswordHash", "Correo", "RolId", "Activo" },
                new object[] { 1, "admin", "$2a$11$rBnqeGmRVmlyS/gLMbB3OuT3tYqMc4.pJeqm0TdE7Q3Ue0HvWkEPi", "admin@gestionprofesoral.edu.co", 1, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Usuarios");
            migrationBuilder.DropTable(name: "Roles");
        }
    }
}
