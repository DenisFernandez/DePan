using Microsoft.EntityFrameworkCore;
using DePan.Data;
using DePan.Models;

namespace DePan.Services
{
    public class CarritoService
    {
        private readonly ApplicationDbContext _context;

        public CarritoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener carrito del usuario actual
        public async Task<Carrito?> GetCarritoUsuarioAsync(int usuarioId)
        {
            return await _context.Carritos
                .Include(c => c.LineaCarritos)
                .ThenInclude(lc => lc.IdProductoNavigation)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);
        }

        // Agregar producto al carrito
        public async Task<bool> AgregarAlCarritoAsync(int usuarioId, int productoId, int cantidad = 1)
        {
            try
            {
                var carrito = await GetCarritoUsuarioAsync(usuarioId);
                var producto = await _context.Productos.FindAsync(productoId);

                if (producto == null) return false;

                // Check stock available before proceeding
                if (producto.Stock < cantidad) return false;

                if (carrito == null)
                {
                    // Crear nuevo carrito si no existe
                    carrito = new Carrito
                    {
                        IdUsuario = usuarioId,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Total = 0
                    };
                    _context.Carritos.Add(carrito);
                    await _context.SaveChangesAsync();
                }

                // Verificar si el producto ya está en el carrito
                var lineaExistente = await _context.LineaCarritos
                    .FirstOrDefaultAsync(lc => lc.IdCarrito == carrito.IdCarrito && lc.IdProducto == productoId);

                if (lineaExistente != null)
                {
                    // Actualizar cantidad si ya existe
                    // Ensure enough stock for the additional units
                    if (producto.Stock < cantidad) return false;

                    lineaExistente.Cantidad += cantidad;
                    lineaExistente.Subtotal = lineaExistente.Cantidad * producto.Precio;
                }
                else
                {
                    // Crear nueva línea de carrito
                    var nuevaLinea = new LineaCarrito
                    {
                        IdCarrito = carrito.IdCarrito,
                        IdProducto = productoId,
                        Cantidad = cantidad,
                        PrecioUnitario = producto.Precio,
                        Subtotal = cantidad * producto.Precio
                    };
                    _context.LineaCarritos.Add(nuevaLinea);
                }
                // Decrement product stock and persist
                producto.Stock -= cantidad;
                if (producto.Stock < 0) producto.Stock = 0;

                // Actualizar total del carrito y fecha
                await ActualizarTotalCarritoAsync(carrito.IdCarrito);
                carrito.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Actualizar cantidad en carrito
        public async Task<bool> ActualizarCantidadCarritoAsync(int lineaCarritoId, int nuevaCantidad)
        {
            try
            {
                var linea = await _context.LineaCarritos
                    .Include(lc => lc.IdProductoNavigation)
                    .FirstOrDefaultAsync(lc => lc.IdLineaCarrito == lineaCarritoId);

                if (linea != null && nuevaCantidad > 0)
                {
                    var producto = linea.IdProductoNavigation;

                    // Calculate difference and adjust stock accordingly
                    var diferencia = nuevaCantidad - linea.Cantidad; // positive => need more units
                    if (diferencia > 0)
                    {
                        if (producto.Stock < diferencia) return false; // not enough stock
                        producto.Stock -= diferencia;
                    }
                    else if (diferencia < 0)
                    {
                        // return stock
                        producto.Stock += -diferencia;
                    }

                    linea.Cantidad = nuevaCantidad;
                    linea.Subtotal = nuevaCantidad * producto.Precio;

                    // Actualizar fecha del carrito
                    var carrito = await _context.Carritos.FindAsync(linea.IdCarrito);
                    if (carrito != null)
                    {
                        carrito.FechaActualizacion = DateTime.Now;
                        await ActualizarTotalCarritoAsync(carrito.IdCarrito);
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

        // Eliminar producto del carrito
        public async Task<bool> EliminarDelCarritoAsync(int lineaCarritoId)
        {
            try
            {
                var linea = await _context.LineaCarritos
                    .Include(lc => lc.IdProductoNavigation)
                    .FirstOrDefaultAsync(lc => lc.IdLineaCarrito == lineaCarritoId);
                if (linea != null)
                {
                    // Return stock to product
                    var producto = linea.IdProductoNavigation;
                    if (producto != null)
                    {
                        producto.Stock += linea.Cantidad;
                    }

                    _context.LineaCarritos.Remove(linea);

                    // Actualizar fecha del carrito
                    var carrito = await _context.Carritos.FindAsync(linea.IdCarrito);
                    if (carrito != null)
                    {
                        carrito.FechaActualizacion = DateTime.Now;
                        await ActualizarTotalCarritoAsync(carrito.IdCarrito);
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

        // Vaciar carrito
        public async Task<bool> VaciarCarritoAsync(int usuarioId)
        {
            try
            {
                var carrito = await GetCarritoUsuarioAsync(usuarioId);
                if (carrito != null)
                {
                    _context.LineaCarritos.RemoveRange(carrito.LineaCarritos);
                    carrito.Total = 0;
                    carrito.FechaActualizacion = DateTime.Now;
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

        // Actualizar total del carrito
        private async Task ActualizarTotalCarritoAsync(int carritoId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.LineaCarritos)
                .FirstOrDefaultAsync(c => c.IdCarrito == carritoId);

            if (carrito != null)
            {
                carrito.Total = carrito.LineaCarritos.Sum(lc => lc.Subtotal);
            }
        }

        // Calcular total del carrito
        public decimal CalcularTotal(Carrito carrito)
        {
            return carrito.LineaCarritos.Sum(lc => lc.Subtotal);
        }

        // Obtener cantidad de items en carrito
        public int ObtenerCantidadItems(Carrito carrito)
        {
            return carrito.LineaCarritos.Sum(lc => lc.Cantidad);
        }
    }
}