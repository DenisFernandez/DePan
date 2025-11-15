using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DePan.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDisponibleFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Marcar todos los productos con stock > 0 como disponibles
            migrationBuilder.Sql("UPDATE producto SET disponible = 1 WHERE stock > 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No hacer nada en el rollback
        }
    }
}
