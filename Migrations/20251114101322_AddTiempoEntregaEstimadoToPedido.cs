using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DePan.Migrations
{
    /// <inheritdoc />
    public partial class AddTiempoEntregaEstimadoToPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_categoria);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellidos = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_postal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rol = table.Column<string>(type: "enum('cliente','repartidor','administrador')", nullable: false, defaultValueSql: "'cliente'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'1'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_usuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_categoria = table.Column<int>(type: "int(11)", nullable: false),
                    nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    imagen_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stock = table.Column<int>(type: "int(11)", nullable: false),
                    disponible = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'1'"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_producto);
                    table.ForeignKey(
                        name: "producto_ibfk_1",
                        column: x => x.id_categoria,
                        principalTable: "categoria",
                        principalColumn: "id_categoria");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "carrito",
                columns: table => new
                {
                    id_carrito = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int(11)", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_carrito);
                    table.ForeignKey(
                        name: "carrito_ibfk_1",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id_pedido = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario_cliente = table.Column<int>(type: "int(11)", nullable: false),
                    id_repartidor = table.Column<int>(type: "int(11)", nullable: true),
                    numero_pedido = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_pedido = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    gastos_envio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    estado = table.Column<string>(type: "enum('pendiente','preparando','enviado','entregado','cancelado')", nullable: false, defaultValueSql: "'pendiente'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion_entrega = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad_entrega = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_postal_entrega = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono_contacto = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    notas = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_entrega_estimada = table.Column<DateTime>(type: "datetime", nullable: true),
                    fecha_entrega_real = table.Column<DateTime>(type: "datetime", nullable: true),
                    TiempoEntregaEstimadoMinutos = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_pedido);
                    table.ForeignKey(
                        name: "pedido_ibfk_1",
                        column: x => x.id_usuario_cliente,
                        principalTable: "usuario",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "pedido_ibfk_2",
                        column: x => x.id_repartidor,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "valoracion",
                columns: table => new
                {
                    id_valoracion = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int(11)", nullable: false),
                    id_producto = table.Column<int>(type: "int(11)", nullable: false),
                    puntuacion = table.Column<int>(type: "int(11)", nullable: false),
                    comentario = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_valoracion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_valoracion);
                    table.ForeignKey(
                        name: "valoracion_ibfk_1",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "valoracion_ibfk_2",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "linea_carrito",
                columns: table => new
                {
                    id_linea_carrito = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_carrito = table.Column<int>(type: "int(11)", nullable: false),
                    id_producto = table.Column<int>(type: "int(11)", nullable: false),
                    cantidad = table.Column<int>(type: "int(11)", nullable: false, defaultValueSql: "'1'"),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    fecha_reserva = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_linea_carrito);
                    table.ForeignKey(
                        name: "linea_carrito_ibfk_1",
                        column: x => x.id_carrito,
                        principalTable: "carrito",
                        principalColumn: "id_carrito",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "linea_carrito_ibfk_2",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id_producto");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "linea_pedido",
                columns: table => new
                {
                    id_linea_pedido = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<int>(type: "int(11)", nullable: false),
                    id_producto = table.Column<int>(type: "int(11)", nullable: false),
                    nombre_producto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Snapshot del nombre en el momento del pedido", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cantidad = table.Column<int>(type: "int(11)", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_linea_pedido);
                    table.ForeignKey(
                        name: "linea_pedido_ibfk_1",
                        column: x => x.id_pedido,
                        principalTable: "pedido",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "linea_pedido_ibfk_2",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id_producto");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "seguimiento_pedido",
                columns: table => new
                {
                    id_seguimiento = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<int>(type: "int(11)", nullable: false),
                    estado = table.Column<string>(type: "enum('pendiente','preparando','enviado','entregado','cancelado')", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_estado = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    observaciones = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    latitud = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: true, comment: "Ubicación del repartidor"),
                    longitud = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: true, comment: "Ubicación del repartidor")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_seguimiento);
                    table.ForeignKey(
                        name: "seguimiento_pedido_ibfk_1",
                        column: x => x.id_pedido,
                        principalTable: "pedido",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "idx_usuario",
                table: "carrito",
                column: "id_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_activa",
                table: "categoria",
                column: "activa");

            migrationBuilder.CreateIndex(
                name: "idx_nombre",
                table: "categoria",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_carrito",
                table: "linea_carrito",
                column: "id_carrito");

            migrationBuilder.CreateIndex(
                name: "idx_producto",
                table: "linea_carrito",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "uk_carrito_producto",
                table: "linea_carrito",
                columns: new[] { "id_carrito", "id_producto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_pedido",
                table: "linea_pedido",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "idx_producto1",
                table: "linea_pedido",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "idx_estado",
                table: "pedido",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_fecha_pedido",
                table: "pedido",
                column: "fecha_pedido");

            migrationBuilder.CreateIndex(
                name: "idx_numero_pedido",
                table: "pedido",
                column: "numero_pedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_repartidor",
                table: "pedido",
                column: "id_repartidor");

            migrationBuilder.CreateIndex(
                name: "idx_usuario_cliente",
                table: "pedido",
                column: "id_usuario_cliente");

            migrationBuilder.CreateIndex(
                name: "idx_busqueda",
                table: "producto",
                columns: new[] { "nombre", "descripcion" })
                .Annotation("MySql:FullTextIndex", true);

            migrationBuilder.CreateIndex(
                name: "idx_categoria",
                table: "producto",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "idx_disponible",
                table: "producto",
                column: "disponible");

            migrationBuilder.CreateIndex(
                name: "idx_nombre1",
                table: "producto",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "idx_estado1",
                table: "seguimiento_pedido",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_fecha_estado",
                table: "seguimiento_pedido",
                column: "fecha_estado");

            migrationBuilder.CreateIndex(
                name: "idx_pedido1",
                table: "seguimiento_pedido",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "email",
                table: "usuario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_activo",
                table: "usuario",
                column: "activo");

            migrationBuilder.CreateIndex(
                name: "idx_rol",
                table: "usuario",
                column: "rol");

            migrationBuilder.CreateIndex(
                name: "idx_fecha",
                table: "valoracion",
                column: "fecha_valoracion");

            migrationBuilder.CreateIndex(
                name: "idx_producto2",
                table: "valoracion",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "idx_puntuacion",
                table: "valoracion",
                column: "puntuacion");

            migrationBuilder.CreateIndex(
                name: "uk_usuario_producto",
                table: "valoracion",
                columns: new[] { "id_usuario", "id_producto" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "linea_carrito");

            migrationBuilder.DropTable(
                name: "linea_pedido");

            migrationBuilder.DropTable(
                name: "seguimiento_pedido");

            migrationBuilder.DropTable(
                name: "valoracion");

            migrationBuilder.DropTable(
                name: "carrito");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "categoria");
        }
    }
}
