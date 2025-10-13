using System;
using System.Collections.Generic;
using DePan.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DePan.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<LineaCarrito> LineaCarritos { get; set; }

    public virtual DbSet<LineaPedido> LineaPedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<SeguimientoPedido> SeguimientoPedidos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VPedidosCompleto> VPedidosCompletos { get; set; }

    public virtual DbSet<VPedidosEnCurso> VPedidosEnCursos { get; set; }

    public virtual DbSet<VProductosMasVendido> VProductosMasVendidos { get; set; }

    public virtual DbSet<Valoracion> Valoracions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=depan_db;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.IdCarrito).HasName("PRIMARY");

            entity.ToTable("carrito");

            entity.HasIndex(e => e.IdUsuario, "idx_usuario").IsUnique();

            entity.Property(e => e.IdCarrito)
                .HasColumnType("int(11)")
                .HasColumnName("id_carrito");
            entity.Property(e => e.FechaActualizacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Carrito)
                .HasForeignKey<Carrito>(d => d.IdUsuario)
                .HasConstraintName("carrito_ibfk_1");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.HasIndex(e => e.Activa, "idx_activa");

            entity.HasIndex(e => e.Nombre, "idx_nombre").IsUnique();

            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("id_categoria");
            entity.Property(e => e.Activa)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("activa");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<LineaCarrito>(entity =>
        {
            entity.HasKey(e => e.IdLineaCarrito).HasName("PRIMARY");

            entity.ToTable("linea_carrito");

            entity.HasIndex(e => e.IdCarrito, "idx_carrito");

            entity.HasIndex(e => e.IdProducto, "idx_producto");

            entity.HasIndex(e => new { e.IdCarrito, e.IdProducto }, "uk_carrito_producto").IsUnique();

            entity.Property(e => e.IdLineaCarrito)
                .HasColumnType("int(11)")
                .HasColumnName("id_linea_carrito");
            entity.Property(e => e.Cantidad)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("cantidad");
            entity.Property(e => e.IdCarrito)
                .HasColumnType("int(11)")
                .HasColumnName("id_carrito");
            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdCarritoNavigation).WithMany(p => p.LineaCarritos)
                .HasForeignKey(d => d.IdCarrito)
                .HasConstraintName("linea_carrito_ibfk_1");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.LineaCarritos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("linea_carrito_ibfk_2");
        });

        modelBuilder.Entity<LineaPedido>(entity =>
        {
            entity.HasKey(e => e.IdLineaPedido).HasName("PRIMARY");

            entity.ToTable("linea_pedido");

            entity.HasIndex(e => e.IdPedido, "idx_pedido");

            entity.HasIndex(e => e.IdProducto, "idx_producto");

            entity.Property(e => e.IdLineaPedido)
                .HasColumnType("int(11)")
                .HasColumnName("id_linea_pedido");
            entity.Property(e => e.Cantidad)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad");
            entity.Property(e => e.IdPedido)
                .HasColumnType("int(11)")
                .HasColumnName("id_pedido");
            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_producto");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(200)
                .HasComment("Snapshot del nombre en el momento del pedido")
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.LineaPedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("linea_pedido_ibfk_1");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.LineaPedidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("linea_pedido_ibfk_2");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.Estado, "idx_estado");

            entity.HasIndex(e => e.FechaPedido, "idx_fecha_pedido");

            entity.HasIndex(e => e.NumeroPedido, "idx_numero_pedido").IsUnique();

            entity.HasIndex(e => e.IdRepartidor, "idx_repartidor");

            entity.HasIndex(e => e.IdUsuarioCliente, "idx_usuario_cliente");

            entity.Property(e => e.IdPedido)
                .HasColumnType("int(11)")
                .HasColumnName("id_pedido");
            entity.Property(e => e.CiudadEntrega)
                .HasMaxLength(100)
                .HasColumnName("ciudad_entrega");
            entity.Property(e => e.CodigoPostalEntrega)
                .HasMaxLength(10)
                .HasColumnName("codigo_postal_entrega");
            entity.Property(e => e.DireccionEntrega)
                .HasMaxLength(255)
                .HasColumnName("direccion_entrega");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'pendiente'")
                .HasColumnType("enum('pendiente','preparando','enviado','entregado','cancelado')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaEntregaEstimada)
                .HasColumnType("datetime")
                .HasColumnName("fecha_entrega_estimada");
            entity.Property(e => e.FechaEntregaReal)
                .HasColumnType("datetime")
                .HasColumnName("fecha_entrega_real");
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pedido");
            entity.Property(e => e.GastosEnvio)
                .HasPrecision(10, 2)
                .HasColumnName("gastos_envio");
            entity.Property(e => e.IdRepartidor)
                .HasColumnType("int(11)")
                .HasColumnName("id_repartidor");
            entity.Property(e => e.IdUsuarioCliente)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario_cliente");
            entity.Property(e => e.Notas)
                .HasColumnType("text")
                .HasColumnName("notas");
            entity.Property(e => e.NumeroPedido)
                .HasMaxLength(50)
                .HasColumnName("numero_pedido");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.TelefonoContacto)
                .HasMaxLength(20)
                .HasColumnName("telefono_contacto");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdRepartidorNavigation).WithMany(p => p.PedidoIdRepartidorNavigations)
                .HasForeignKey(d => d.IdRepartidor)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pedido_ibfk_2");

            entity.HasOne(d => d.IdUsuarioClienteNavigation).WithMany(p => p.PedidoIdUsuarioClienteNavigations)
                .HasForeignKey(d => d.IdUsuarioCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedido_ibfk_1");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => new { e.Nombre, e.Descripcion }, "idx_busqueda").HasAnnotation("MySql:FullTextIndex", true);

            entity.HasIndex(e => e.IdCategoria, "idx_categoria");

            entity.HasIndex(e => e.Disponible, "idx_disponible");

            entity.HasIndex(e => e.Nombre, "idx_nombre");

            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_producto");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Disponible)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("disponible");
            entity.Property(e => e.FechaActualizacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("id_categoria");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(500)
                .HasColumnName("imagen_url");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Stock)
                .HasColumnType("int(11)")
                .HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_ibfk_1");
        });

        modelBuilder.Entity<SeguimientoPedido>(entity =>
        {
            entity.HasKey(e => e.IdSeguimiento).HasName("PRIMARY");

            entity.ToTable("seguimiento_pedido");

            entity.HasIndex(e => e.Estado, "idx_estado");

            entity.HasIndex(e => e.FechaEstado, "idx_fecha_estado");

            entity.HasIndex(e => e.IdPedido, "idx_pedido");

            entity.Property(e => e.IdSeguimiento)
                .HasColumnType("int(11)")
                .HasColumnName("id_seguimiento");
            entity.Property(e => e.Estado)
                .HasColumnType("enum('pendiente','preparando','enviado','entregado','cancelado')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaEstado)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_estado");
            entity.Property(e => e.IdPedido)
                .HasColumnType("int(11)")
                .HasColumnName("id_pedido");
            entity.Property(e => e.Latitud)
                .HasPrecision(10, 8)
                .HasComment("Ubicación del repartidor")
                .HasColumnName("latitud");
            entity.Property(e => e.Longitud)
                .HasPrecision(11, 8)
                .HasComment("Ubicación del repartidor")
                .HasColumnName("longitud");
            entity.Property(e => e.Observaciones)
                .HasColumnType("text")
                .HasColumnName("observaciones");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.SeguimientoPedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("seguimiento_pedido_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Activo, "idx_activo");

            entity.HasIndex(e => e.Rol, "idx_rol");

            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Activo)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(150)
                .HasColumnName("apellidos");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .HasColumnName("ciudad");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("'cliente'")
                .HasColumnType("enum('cliente','repartidor','administrador')")
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<VPedidosCompleto>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_pedidos_completos");

            entity.Property(e => e.ClienteApellidos)
                .HasMaxLength(150)
                .HasColumnName("cliente_apellidos");
            entity.Property(e => e.ClienteEmail)
                .HasMaxLength(255)
                .HasColumnName("cliente_email");
            entity.Property(e => e.ClienteNombre)
                .HasMaxLength(100)
                .HasColumnName("cliente_nombre");
            entity.Property(e => e.ClienteTelefono)
                .HasMaxLength(20)
                .HasColumnName("cliente_telefono");
            entity.Property(e => e.DireccionEntrega)
                .HasMaxLength(255)
                .HasColumnName("direccion_entrega");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'pendiente'")
                .HasColumnType("enum('pendiente','preparando','enviado','entregado','cancelado')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pedido");
            entity.Property(e => e.IdPedido)
                .HasColumnType("int(11)")
                .HasColumnName("id_pedido");
            entity.Property(e => e.NumeroPedido)
                .HasMaxLength(50)
                .HasColumnName("numero_pedido");
            entity.Property(e => e.RepartidorApellidos)
                .HasMaxLength(150)
                .HasColumnName("repartidor_apellidos");
            entity.Property(e => e.RepartidorNombre)
                .HasMaxLength(100)
                .HasColumnName("repartidor_nombre");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");
        });

        modelBuilder.Entity<VPedidosEnCurso>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_pedidos_en_curso");

            entity.Property(e => e.Cliente)
                .HasMaxLength(100)
                .HasColumnName("cliente");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'pendiente'")
                .HasColumnType("enum('pendiente','preparando','enviado','entregado','cancelado')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pedido");
            entity.Property(e => e.IdPedido)
                .HasColumnType("int(11)")
                .HasColumnName("id_pedido");
            entity.Property(e => e.Latitud)
                .HasPrecision(10, 8)
                .HasComment("Ubicación del repartidor")
                .HasColumnName("latitud");
            entity.Property(e => e.Longitud)
                .HasPrecision(11, 8)
                .HasComment("Ubicación del repartidor")
                .HasColumnName("longitud");
            entity.Property(e => e.NumeroPedido)
                .HasMaxLength(50)
                .HasColumnName("numero_pedido");
            entity.Property(e => e.Repartidor)
                .HasMaxLength(100)
                .HasColumnName("repartidor");
            entity.Property(e => e.UltimaActualizacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("ultima_actualizacion");
        });

        modelBuilder.Entity<VProductosMasVendido>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_productos_mas_vendidos");

            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .HasColumnName("categoria");
            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_producto");
            entity.Property(e => e.IngresosTotales)
                .HasPrecision(32, 2)
                .HasColumnName("ingresos_totales");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.TotalVentas)
                .HasColumnType("bigint(21)")
                .HasColumnName("total_ventas");
            entity.Property(e => e.UnidadesVendidas)
                .HasPrecision(32)
                .HasColumnName("unidades_vendidas");
        });

        modelBuilder.Entity<Valoracion>(entity =>
        {
            entity.HasKey(e => e.IdValoracion).HasName("PRIMARY");

            entity.ToTable("valoracion");

            entity.HasIndex(e => e.FechaValoracion, "idx_fecha");

            entity.HasIndex(e => e.IdProducto, "idx_producto");

            entity.HasIndex(e => e.Puntuacion, "idx_puntuacion");

            entity.HasIndex(e => new { e.IdUsuario, e.IdProducto }, "uk_usuario_producto").IsUnique();

            entity.Property(e => e.IdValoracion)
                .HasColumnType("int(11)")
                .HasColumnName("id_valoracion");
            entity.Property(e => e.Comentario)
                .HasColumnType("text")
                .HasColumnName("comentario");
            entity.Property(e => e.FechaValoracion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_valoracion");
            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_producto");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Puntuacion)
                .HasColumnType("int(11)")
                .HasColumnName("puntuacion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("valoracion_ibfk_2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Valoracions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("valoracion_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
