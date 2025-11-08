using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DePan.Data;
using DePan.Models;

namespace DePan.Controllers
{
    [Authorize(Roles = "Administrador,administrador")]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Reportes
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Reportes/GetVentasPorMes
        [HttpGet]
        public async Task<IActionResult> GetVentasPorMes(int meses = 6)
        {
            var fechaInicio = DateTime.Now.AddMonths(-meses);

            var ventas = await _context.Pedidos
                .Where(p => p.FechaPedido >= fechaInicio && p.Estado != "cancelado")
                .GroupBy(p => new { p.FechaPedido.Year, p.FechaPedido.Month })
                .Select(g => new
                {
                    Fecha = new DateTime(g.Key.Year, g.Key.Month, 1),
                    TotalVentas = g.Sum(p => p.Total),
                    NumPedidos = g.Count()
                })
                .OrderBy(x => x.Fecha)
                .ToListAsync();

            return Json(new
            {
                labels = ventas.Select(v => v.Fecha.ToString("MMM yyyy")).ToList(),
                ventas = ventas.Select(v => v.TotalVentas).ToList(),
                pedidos = ventas.Select(v => v.NumPedidos).ToList()
            });
        }

        // GET: /Reportes/GetProductosMasVendidos
        [HttpGet]
        public async Task<IActionResult> GetProductosMasVendidos(int top = 10)
        {
            var productos = await _context.LineaPedidos
                .Include(lp => lp.IdProductoNavigation)
                .GroupBy(lp => new { lp.IdProducto, lp.IdProductoNavigation.Nombre })
                .Select(g => new
                {
                    Producto = g.Key.Nombre,
                    CantidadVendida = g.Sum(lp => lp.Cantidad),
                    TotalIngresos = g.Sum(lp => lp.Subtotal)
                })
                .OrderByDescending(x => x.CantidadVendida)
                .Take(top)
                .ToListAsync();

            return Json(new
            {
                productos = productos.Select(p => p.Producto).ToList(),
                cantidades = productos.Select(p => p.CantidadVendida).ToList(),
                ingresos = productos.Select(p => p.TotalIngresos).ToList()
            });
        }

        // GET: /Reportes/GetEstadisticasGenerales
        [HttpGet]
        public async Task<IActionResult> GetEstadisticasGenerales()
        {
            var hoy = DateTime.Now;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var inicioAnio = new DateTime(hoy.Year, 1, 1);

            var stats = new
            {
                TotalClientes = await _context.Usuarios.CountAsync(),
                TotalPedidos = await _context.Pedidos.CountAsync(),
                PedidosMes = await _context.Pedidos
                    .Where(p => p.FechaPedido >= inicioMes)
                    .CountAsync(),
                VentasMes = await _context.Pedidos
                    .Where(p => p.FechaPedido >= inicioMes && p.Estado != "cancelado")
                    .SumAsync(p => (decimal?)p.Total) ?? 0,
                VentasAnio = await _context.Pedidos
                    .Where(p => p.FechaPedido >= inicioAnio && p.Estado != "cancelado")
                    .SumAsync(p => (decimal?)p.Total) ?? 0,
                ProductosActivos = await _context.Productos
                    .Where(p => p.Disponible == true)
                    .CountAsync(),
                ProductosBajoStock = await _context.Productos
                    .Where(p => p.Stock < 10)
                    .CountAsync()
            };

            return Json(stats);
        }

        // GET: /Reportes/GetAnalisisClientes
        [HttpGet]
        public async Task<IActionResult> GetAnalisisClientes()
        {
            var clientesConCompras = await _context.Pedidos
                .Include(p => p.IdUsuarioClienteNavigation)
                .GroupBy(p => new { p.IdUsuarioCliente, p.IdUsuarioClienteNavigation.Email })
                .Select(g => new
                {
                    Email = g.Key.Email,
                    TotalPedidos = g.Count(),
                    TotalGastado = g.Sum(p => p.Total),
                    UltimaCompra = g.Max(p => p.FechaPedido)
                })
                .OrderByDescending(x => x.TotalGastado)
                .Take(10)
                .ToListAsync();

            return Json(clientesConCompras);
        }

        // GET: /Reportes/GetVentasPorCategoria
        [HttpGet]
        public async Task<IActionResult> GetVentasPorCategoria()
        {
            var ventasPorCategoria = await _context.LineaPedidos
                .Include(lp => lp.IdProductoNavigation)
                .ThenInclude(p => p.IdCategoriaNavigation)
                .GroupBy(lp => lp.IdProductoNavigation.IdCategoriaNavigation.Nombre)
                .Select(g => new
                {
                    Categoria = g.Key,
                    TotalVentas = g.Sum(lp => lp.Subtotal)
                })
                .OrderByDescending(x => x.TotalVentas)
                .ToListAsync();

            return Json(new
            {
                categorias = ventasPorCategoria.Select(c => c.Categoria).ToList(),
                ventas = ventasPorCategoria.Select(c => c.TotalVentas).ToList()
            });
        }

        // GET: /Reportes/GetEstadosPedidos
        [HttpGet]
        public async Task<IActionResult> GetEstadosPedidos()
        {
            var estadosPedidos = await _context.Pedidos
                .GroupBy(p => p.Estado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                })
                .ToListAsync();

            return Json(new
            {
                estados = estadosPedidos.Select(e => e.Estado).ToList(),
                cantidades = estadosPedidos.Select(e => e.Cantidad).ToList()
            });
        }
    }
}
