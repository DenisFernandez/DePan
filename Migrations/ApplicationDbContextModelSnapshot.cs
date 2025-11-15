using System;
using DePan.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DePan.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_unicode_ci")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("DePan.Models.Carrito", b =>
                {
                    b.Property<int>("IdCarrito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_carrito");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdCarrito"));

                    b.Property<DateTime>("FechaActualizacion")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_actualizacion")
                        .HasDefaultValueSql("current_timestamp()");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("FechaActualizacion"));

                    b.Property<DateTime>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_creacion")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_usuario");

                    b.Property<decimal>("Total")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("total");

                    b.HasKey("IdCarrito")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdUsuario" }, "idx_usuario")
                        .IsUnique();

                    b.ToTable("carrito", (string)null);
                });

            modelBuilder.Entity("DePan.Models.Categorium", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_categoria");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdCategoria"));

                    b.Property<bool?>("Activa")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("activa")
                        .HasDefaultValueSql("'1'");

                    b.Property<string>("Descripcion")
                        .HasColumnType("text")
                        .HasColumnName("descripcion");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nombre");

                    b.HasKey("IdCategoria")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Activa" }, "idx_activa");

                    b.HasIndex(new[] { "Nombre" }, "idx_nombre")
                        .IsUnique();

                    b.ToTable("categoria", (string)null);
                });

            modelBuilder.Entity("DePan.Models.LineaCarrito", b =>
                {
                    b.Property<int>("IdLineaCarrito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_linea_carrito");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdLineaCarrito"));

                    b.Property<int>("Cantidad")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("cantidad")
                        .HasDefaultValueSql("'1'");

                    b.Property<DateTime>("FechaReserva")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("fecha_reserva");

                    b.Property<int>("IdCarrito")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_carrito");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_producto");

                    b.Property<decimal>("PrecioUnitario")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("precio_unitario");

                    b.Property<decimal>("Subtotal")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("subtotal");

                    b.HasKey("IdLineaCarrito")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdCarrito" }, "idx_carrito");

                    b.HasIndex(new[] { "IdProducto" }, "idx_producto");

                    b.HasIndex(new[] { "IdCarrito", "IdProducto" }, "uk_carrito_producto")
                        .IsUnique();

                    b.ToTable("linea_carrito", (string)null);
                });

            modelBuilder.Entity("DePan.Models.LineaPedido", b =>
                {
                    b.Property<int>("IdLineaPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_linea_pedido");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdLineaPedido"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int(11)")
                        .HasColumnName("cantidad");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_pedido");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_producto");

                    b.Property<string>("NombreProducto")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("nombre_producto")
                        .HasComment("Snapshot del nombre en el momento del pedido");

                    b.Property<decimal>("PrecioUnitario")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("precio_unitario");

                    b.Property<decimal>("Subtotal")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("subtotal");

                    b.HasKey("IdLineaPedido")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdPedido" }, "idx_pedido");

                    b.HasIndex(new[] { "IdProducto" }, "idx_producto")
                        .HasDatabaseName("idx_producto1");

                    b.ToTable("linea_pedido", (string)null);
                });

            modelBuilder.Entity("DePan.Models.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_pedido");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdPedido"));

                    b.Property<string>("CiudadEntrega")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ciudad_entrega");

                    b.Property<string>("CodigoPostalEntrega")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("codigo_postal_entrega");

                    b.Property<string>("DireccionEntrega")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("direccion_entrega");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("enum('pendiente','preparando','enviado','entregado','retrasado','cancelado')")
                        .HasColumnName("estado")
                        .HasDefaultValueSql("'pendiente'");

                    b.Property<DateTime?>("FechaEntregaEstimada")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_entrega_estimada");

                    b.Property<DateTime?>("FechaEntregaReal")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_entrega_real");

                    b.Property<DateTime>("FechaPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_pedido")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<decimal>("GastosEnvio")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("gastos_envio");

                    b.Property<int?>("IdRepartidor")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_repartidor");

                    b.Property<int>("IdUsuarioCliente")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_usuario_cliente");

                    b.Property<string>("Notas")
                        .HasColumnType("text")
                        .HasColumnName("notas");

                    b.Property<string>("NumeroPedido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("numero_pedido");

                    b.Property<decimal>("Subtotal")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("subtotal");

                    b.Property<string>("TelefonoContacto")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("telefono_contacto");

                    b.Property<decimal>("Total")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("total");

                    b.HasKey("IdPedido")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Estado" }, "idx_estado");

                    b.HasIndex(new[] { "FechaPedido" }, "idx_fecha_pedido");

                    b.HasIndex(new[] { "NumeroPedido" }, "idx_numero_pedido")
                        .IsUnique();

                    b.HasIndex(new[] { "IdRepartidor" }, "idx_repartidor");

                    b.HasIndex(new[] { "IdUsuarioCliente" }, "idx_usuario_cliente");

                    b.ToTable("pedido", (string)null);
                });

            modelBuilder.Entity("DePan.Models.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_producto");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdProducto"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("text")
                        .HasColumnName("descripcion");

                    b.Property<bool?>("Disponible")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("disponible")
                        .HasDefaultValueSql("'1'");

                    b.Property<DateTime>("FechaActualizacion")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_actualizacion")
                        .HasDefaultValueSql("current_timestamp()");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("FechaActualizacion"));

                    b.Property<DateTime>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_creacion")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int>("IdCategoria")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_categoria");

                    b.Property<string>("ImagenUrl")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("imagen_url");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("nombre");

                    b.Property<decimal>("Precio")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("precio");

                    b.Property<int>("Stock")
                        .HasColumnType("int(11)")
                        .HasColumnName("stock");

                    b.HasKey("IdProducto")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Nombre", "Descripcion" }, "idx_busqueda")
                        .HasAnnotation("MySql:FullTextIndex", true);

                    b.HasIndex(new[] { "IdCategoria" }, "idx_categoria");

                    b.HasIndex(new[] { "Disponible" }, "idx_disponible");

                    b.HasIndex(new[] { "Nombre" }, "idx_nombre")
                        .HasDatabaseName("idx_nombre1");

                    b.ToTable("producto", (string)null);
                });

            modelBuilder.Entity("DePan.Models.SeguimientoPedido", b =>
                {
                    b.Property<int>("IdSeguimiento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_seguimiento");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdSeguimiento"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("enum('pendiente','preparando','enviado','entregado','cancelado')")
                        .HasColumnName("estado");

                    b.Property<DateTime>("FechaEstado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_estado")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_pedido");

                    b.Property<decimal?>("Latitud")
                        .HasPrecision(10, 8)
                        .HasColumnType("decimal(10,8)")
                        .HasColumnName("latitud")
                        .HasComment("Ubicación del repartidor");

                    b.Property<decimal?>("Longitud")
                        .HasPrecision(11, 8)
                        .HasColumnType("decimal(11,8)")
                        .HasColumnName("longitud")
                        .HasComment("Ubicación del repartidor");

                    b.Property<string>("Observaciones")
                        .HasColumnType("text")
                        .HasColumnName("observaciones");

                    b.HasKey("IdSeguimiento")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Estado" }, "idx_estado")
                        .HasDatabaseName("idx_estado1");

                    b.HasIndex(new[] { "FechaEstado" }, "idx_fecha_estado");

                    b.HasIndex(new[] { "IdPedido" }, "idx_pedido")
                        .HasDatabaseName("idx_pedido1");

                    b.ToTable("seguimiento_pedido", (string)null);
                });

            modelBuilder.Entity("DePan.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_usuario");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<bool?>("Activo")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("activo")
                        .HasDefaultValueSql("'1'");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("apellidos");

                    b.Property<string>("Ciudad")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ciudad");

                    b.Property<string>("CodigoPostal")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("codigo_postal");

                    b.Property<string>("Direccion")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("direccion");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<DateTime>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_registro")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nombre");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password_hash");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("enum('cliente','repartidor','administrador')")
                        .HasColumnName("rol")
                        .HasDefaultValueSql("'cliente'");

                    b.Property<string>("Telefono")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("telefono");

                    b.HasKey("IdUsuario")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Email" }, "email")
                        .IsUnique();

                    b.HasIndex(new[] { "Activo" }, "idx_activo");

                    b.HasIndex(new[] { "Rol" }, "idx_rol");

                    b.ToTable("usuario", (string)null);
                });

            modelBuilder.Entity("DePan.Models.VPedidosCompleto", b =>
                {
                    b.Property<string>("ClienteApellidos")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("cliente_apellidos");

                    b.Property<string>("ClienteEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("cliente_email");

                    b.Property<string>("ClienteNombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("cliente_nombre");

                    b.Property<string>("ClienteTelefono")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("cliente_telefono");

                    b.Property<string>("DireccionEntrega")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("direccion_entrega");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("enum('pendiente','preparando','enviado','entregado','cancelado')")
                        .HasColumnName("estado")
                        .HasDefaultValueSql("'pendiente'");

                    b.Property<DateTime>("FechaPedido")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_pedido")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_pedido");

                    b.Property<string>("NumeroPedido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("numero_pedido");

                    b.Property<string>("RepartidorApellidos")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("repartidor_apellidos");

                    b.Property<string>("RepartidorNombre")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("repartidor_nombre");

                    b.Property<decimal>("Total")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("total");

                    b.ToTable((string)null);

                    b.ToView("v_pedidos_completos", (string)null);
                });

            modelBuilder.Entity("DePan.Models.VPedidosEnCurso", b =>
                {
                    b.Property<string>("Cliente")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("cliente");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("enum('pendiente','preparando','enviado','entregado','cancelado')")
                        .HasColumnName("estado")
                        .HasDefaultValueSql("'pendiente'");

                    b.Property<DateTime>("FechaPedido")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_pedido")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_pedido");

                    b.Property<decimal?>("Latitud")
                        .HasPrecision(10, 8)
                        .HasColumnType("decimal(10,8)")
                        .HasColumnName("latitud")
                        .HasComment("Ubicación del repartidor");

                    b.Property<decimal?>("Longitud")
                        .HasPrecision(11, 8)
                        .HasColumnType("decimal(11,8)")
                        .HasColumnName("longitud")
                        .HasComment("Ubicación del repartidor");

                    b.Property<string>("NumeroPedido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("numero_pedido");

                    b.Property<string>("Repartidor")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("repartidor");

                    b.Property<DateTime?>("UltimaActualizacion")
                        .HasColumnType("datetime")
                        .HasColumnName("ultima_actualizacion")
                        .HasDefaultValueSql("current_timestamp()");

                    b.ToTable((string)null);

                    b.ToView("v_pedidos_en_curso", (string)null);
                });

            modelBuilder.Entity("DePan.Models.VProductosMasVendido", b =>
                {
                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("categoria");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_producto");

                    b.Property<decimal?>("IngresosTotales")
                        .HasPrecision(32, 2)
                        .HasColumnType("decimal(32,2)")
                        .HasColumnName("ingresos_totales");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("nombre");

                    b.Property<decimal>("Precio")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("precio");

                    b.Property<long>("TotalVentas")
                        .HasColumnType("bigint(21)")
                        .HasColumnName("total_ventas");

                    b.Property<decimal?>("UnidadesVendidas")
                        .HasPrecision(32)
                        .HasColumnType("decimal(32)")
                        .HasColumnName("unidades_vendidas");

                    b.ToTable((string)null);

                    b.ToView("v_productos_mas_vendidos", (string)null);
                });

            modelBuilder.Entity("DePan.Models.Valoracion", b =>
                {
                    b.Property<int>("IdValoracion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id_valoracion");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdValoracion"));

                    b.Property<string>("Comentario")
                        .HasColumnType("text")
                        .HasColumnName("comentario");

                    b.Property<DateTime>("FechaValoracion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha_valoracion")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_producto");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int(11)")
                        .HasColumnName("id_usuario");

                    b.Property<int>("Puntuacion")
                        .HasColumnType("int(11)")
                        .HasColumnName("puntuacion");

                    b.HasKey("IdValoracion")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "FechaValoracion" }, "idx_fecha");

                    b.HasIndex(new[] { "IdProducto" }, "idx_producto")
                        .HasDatabaseName("idx_producto2");

                    b.HasIndex(new[] { "Puntuacion" }, "idx_puntuacion");

                    b.HasIndex(new[] { "IdUsuario", "IdProducto" }, "uk_usuario_producto")
                        .IsUnique();

                    b.ToTable("valoracion", (string)null);
                });

            modelBuilder.Entity("DePan.Models.Carrito", b =>
                {
                    b.HasOne("DePan.Models.Usuario", "IdUsuarioNavigation")
                        .WithOne("Carrito")
                        .HasForeignKey("DePan.Models.Carrito", "IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("carrito_ibfk_1");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("DePan.Models.LineaCarrito", b =>
                {
                    b.HasOne("DePan.Models.Carrito", "IdCarritoNavigation")
                        .WithMany("LineaCarritos")
                        .HasForeignKey("IdCarrito")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("linea_carrito_ibfk_1");

                    b.HasOne("DePan.Models.Producto", "IdProductoNavigation")
                        .WithMany("LineaCarritos")
                        .HasForeignKey("IdProducto")
                        .IsRequired()
                        .HasConstraintName("linea_carrito_ibfk_2");

                    b.Navigation("IdCarritoNavigation");

                    b.Navigation("IdProductoNavigation");
                });

            modelBuilder.Entity("DePan.Models.LineaPedido", b =>
                {
                    b.HasOne("DePan.Models.Pedido", "IdPedidoNavigation")
                        .WithMany("LineaPedidos")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("linea_pedido_ibfk_1");

                    b.HasOne("DePan.Models.Producto", "IdProductoNavigation")
                        .WithMany("LineaPedidos")
                        .HasForeignKey("IdProducto")
                        .IsRequired()
                        .HasConstraintName("linea_pedido_ibfk_2");

                    b.Navigation("IdPedidoNavigation");

                    b.Navigation("IdProductoNavigation");
                });

            modelBuilder.Entity("DePan.Models.Pedido", b =>
                {
                    b.HasOne("DePan.Models.Usuario", "IdRepartidorNavigation")
                        .WithMany("PedidoIdRepartidorNavigations")
                        .HasForeignKey("IdRepartidor")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("pedido_ibfk_2");

                    b.HasOne("DePan.Models.Usuario", "IdUsuarioClienteNavigation")
                        .WithMany("PedidoIdUsuarioClienteNavigations")
                        .HasForeignKey("IdUsuarioCliente")
                        .IsRequired()
                        .HasConstraintName("pedido_ibfk_1");

                    b.Navigation("IdRepartidorNavigation");

                    b.Navigation("IdUsuarioClienteNavigation");
                });

            modelBuilder.Entity("DePan.Models.Producto", b =>
                {
                    b.HasOne("DePan.Models.Categorium", "IdCategoriaNavigation")
                        .WithMany("Productos")
                        .HasForeignKey("IdCategoria")
                        .IsRequired()
                        .HasConstraintName("producto_ibfk_1");

                    b.Navigation("IdCategoriaNavigation");
                });

            modelBuilder.Entity("DePan.Models.SeguimientoPedido", b =>
                {
                    b.HasOne("DePan.Models.Pedido", "IdPedidoNavigation")
                        .WithMany("SeguimientoPedidos")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("seguimiento_pedido_ibfk_1");

                    b.Navigation("IdPedidoNavigation");
                });

            modelBuilder.Entity("DePan.Models.Valoracion", b =>
                {
                    b.HasOne("DePan.Models.Producto", "IdProductoNavigation")
                        .WithMany("Valoracions")
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("valoracion_ibfk_2");

                    b.HasOne("DePan.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Valoracions")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("valoracion_ibfk_1");

                    b.Navigation("IdProductoNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("DePan.Models.Carrito", b =>
                {
                    b.Navigation("LineaCarritos");
                });

            modelBuilder.Entity("DePan.Models.Categorium", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("DePan.Models.Pedido", b =>
                {
                    b.Navigation("LineaPedidos");

                    b.Navigation("SeguimientoPedidos");
                });

            modelBuilder.Entity("DePan.Models.Producto", b =>
                {
                    b.Navigation("LineaCarritos");

                    b.Navigation("LineaPedidos");

                    b.Navigation("Valoracions");
                });

            modelBuilder.Entity("DePan.Models.Usuario", b =>
                {
                    b.Navigation("Carrito");

                    b.Navigation("PedidoIdRepartidorNavigations");

                    b.Navigation("PedidoIdUsuarioClienteNavigations");

                    b.Navigation("Valoracions");
                });
#pragma warning restore 612, 618
        }
    }
}
