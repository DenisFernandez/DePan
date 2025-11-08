using Microsoft.EntityFrameworkCore;
using DePan.Data;

namespace DePan.Services
{
    public class CarritoCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CarritoCleanupService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30); // Run every 30 seconds (testing)
        private readonly TimeSpan _reservationExpiration = TimeSpan.FromMinutes(2); // Expire after 2 min (testing)

        public CarritoCleanupService(IServiceProvider serviceProvider, ILogger<CarritoCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CarritoCleanupService iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await LiberarReservasExpiradasAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al limpiar reservas expiradas");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("CarritoCleanupService detenido");
        }

        private async Task LiberarReservasExpiradasAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tiempoExpiracion = DateTime.Now.Add(-_reservationExpiration);

            var lineasExpiradas = await context.LineaCarritos
                .Include(lc => lc.IdProductoNavigation)
                .Include(lc => lc.IdCarritoNavigation)
                .Where(lc => lc.FechaReserva < tiempoExpiracion)
                .ToListAsync();

            if (lineasExpiradas.Any())
            {
                _logger.LogInformation($"Liberando {lineasExpiradas.Count} reservas expiradas");

                foreach (var linea in lineasExpiradas)
                {
                    // Devolver stock al producto
                    if (linea.IdProductoNavigation != null)
                    {
                        linea.IdProductoNavigation.Stock += linea.Cantidad;
                        _logger.LogInformation(
                            $"Stock devuelto: Producto {linea.IdProducto} (+{linea.Cantidad} unidades)");
                    }

                    // Eliminar línea expirada
                    context.LineaCarritos.Remove(linea);

                    // Actualizar total del carrito
                    if (linea.IdCarritoNavigation != null)
                    {
                        var carrito = linea.IdCarritoNavigation;
                        carrito.FechaActualizacion = DateTime.Now;
                        // El total se recalculará en la próxima consulta o podemos hacerlo aquí
                        await ActualizarTotalCarritoAsync(context, carrito.IdCarrito);
                    }
                }

                await context.SaveChangesAsync();
                _logger.LogInformation("Limpieza de reservas completada exitosamente");
            }
        }

        private async Task ActualizarTotalCarritoAsync(ApplicationDbContext context, int carritoId)
        {
            var carrito = await context.Carritos
                .Include(c => c.LineaCarritos)
                .FirstOrDefaultAsync(c => c.IdCarrito == carritoId);

            if (carrito != null)
            {
                carrito.Total = carrito.LineaCarritos.Sum(lc => lc.Subtotal);
            }
        }
    }
}
