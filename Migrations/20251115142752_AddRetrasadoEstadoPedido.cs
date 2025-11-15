using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DePan.Migrations
{
    /// <inheritdoc />
    public partial class AddRetrasadoEstadoPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TiempoEntregaEstimadoMinutos",
                table: "pedido");

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "pedido",
                type: "enum('pendiente','preparando','enviado','entregado','retrasado','cancelado')",
                nullable: false,
                defaultValueSql: "'pendiente'",
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "enum('pendiente','preparando','enviado','entregado','cancelado')",
                oldDefaultValueSql: "'pendiente'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "pedido",
                type: "enum('pendiente','preparando','enviado','entregado','cancelado')",
                nullable: false,
                defaultValueSql: "'pendiente'",
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "enum('pendiente','preparando','enviado','entregado','retrasado','cancelado')",
                oldDefaultValueSql: "'pendiente'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<int>(
                name: "TiempoEntregaEstimadoMinutos",
                table: "pedido",
                type: "int",
                nullable: true);
        }
    }
}
