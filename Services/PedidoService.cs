using Microsoft.EntityFrameworkCore;
using DePan.Data;
using DePan.Models;

namespace DePan.Services
{
    public class PedidoService
    {
        private readonly ApplicationDbContext _context;

        public PedidoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Crear pedido desde carrito
        public async Task<Pedido?> CrearPedidoDesdeCarritoAsync(int usuarioId, string direccionEntrega, string ciudadEntrega, string codigoPostal, string telefonoContacto, string? notas = null)
        {
            try
            {
                var carrito = await _context.Carritos
                    .Include(c => c.LineaCarritos)
                    .ThenInclude(lc => lc.IdProductoNavigation)
                    .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);

                if (carrito == null || !carrito.LineaCarritos.Any())
                {
                    return null;
                }

                // Generar número de pedido único
                var numeroPedido = $"DP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";

                // Crear pedido
                var pedido = new Pedido
                {
                    IdUsuarioCliente = usuarioId,
                    NumeroPedido = numeroPedido,
                    FechaPedido = DateTime.Now,
                    Estado = "Pendiente",
                    DireccionEntrega = direccionEntrega,
                    CiudadEntrega = ciudadEntrega,
                    CodigoPostalEntrega = codigoPostal,
                    TelefonoContacto = telefonoContacto,
                    Notas = notas,
                    Subtotal = carrito.Total,
                    GastosEnvio = 2.50m, // Gastos de envío fijos
                    Total = carrito.Total + 2.50m,
                    FechaEntregaEstimada = DateTime.Now.AddDays(2)
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Crear líneas de pedido
                foreach (var lineaCarrito in carrito.LineaCarritos)
                {
                    var lineaPedido = new LineaPedido
                    {
                        IdPedido = pedido.IdPedido,
                        IdProducto = lineaCarrito.IdProducto,
                        Cantidad = lineaCarrito.Cantidad,
                        PrecioUnitario = lineaCarrito.PrecioUnitario,
                        Subtotal = lineaCarrito.Subtotal
                    };
                    _context.LineaPedidos.Add(lineaPedido);

                    // Actualizar stock del producto
                    var producto = await _context.Productos.FindAsync(lineaCarrito.IdProducto);
                    if (producto != null)
                    {
                        producto.Stock -= lineaCarrito.Cantidad;
                        if (producto.Stock < 0) producto.Stock = 0;
                    }
                }

                // Vaciar carrito
                _context.LineaCarritos.RemoveRange(carrito.LineaCarritos);
                carrito.Total = 0;
                carrito.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                return pedido;
            }
            catch
            {
                return null;
            }
        }

        // Obtener pedidos del usuario
        public async Task<List<Pedido>> GetPedidosUsuarioAsync(int usuarioId)
        {
            return await _context.Pedidos
                .Include(p => p.LineaPedidos)
                .ThenInclude(lp => lp.IdProductoNavigation)
                .Where(p => p.IdUsuarioCliente == usuarioId)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();
        }

        // Obtener todos los pedidos (para administradores)
        public async Task<List<Pedido>> GetAllPedidosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.LineaPedidos)
                .ThenInclude(lp => lp.IdProductoNavigation)
                .Include(p => p.IdUsuarioClienteNavigation)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();
        }

        // Obtener pedido por ID
        public async Task<Pedido?> GetPedidoByIdAsync(int pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.LineaPedidos)
                .ThenInclude(lp => lp.IdProductoNavigation)
                .Include(p => p.IdUsuarioClienteNavigation)
                .Include(p => p.IdRepartidorNavigation)
                .FirstOrDefaultAsync(p => p.IdPedido == pedidoId);
        }

        // Actualizar estado del pedido
        public async Task<bool> ActualizarEstadoPedidoAsync(int pedidoId, string nuevoEstado, int? idRepartidor = null)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(pedidoId);
                if (pedido != null)
                {
                    pedido.Estado = nuevoEstado;
                    if (idRepartidor.HasValue)
                    {
                        pedido.IdRepartidor = idRepartidor.Value;
                    }

                    // Si el estado es "Entregado", establecer fecha de entrega real
                    if (nuevoEstado == "Entregado")
                    {
                        pedido.FechaEntregaReal = DateTime.Now;
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Obtener pedidos por estado
        public async Task<List<Pedido>> GetPedidosPorEstadoAsync(string estado)
        {
            return await _context.Pedidos
                .Include(p => p.LineaPedidos)
                .ThenInclude(lp => lp.IdProductoNavigation)
                .Include(p => p.IdUsuarioClienteNavigation)
                .Where(p => p.Estado == estado)
                .OrderBy(p => p.FechaPedido)
                .ToListAsync();
        }

        // Obtener estadísticas de pedidos
        public async Task<object> GetEstadisticasPedidosAsync()
        {
            var totalPedidos = await _context.Pedidos.CountAsync();
            var pedidosHoy = await _context.Pedidos
                .Where(p => p.FechaPedido.Date == DateTime.Today)
                .CountAsync();
            var pedidosPendientes = await _context.Pedidos
                .Where(p => p.Estado == "Pendiente")
                .CountAsync();
            var ingresosTotales = await _context.Pedidos.SumAsync(p => p.Total);

            return new
            {
                TotalPedidos = totalPedidos,
                PedidosHoy = pedidosHoy,
                PedidosPendientes = pedidosPendientes,
                IngresosTotales = ingresosTotales
            };
        }
    }
}