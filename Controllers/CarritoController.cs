using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DePan.Services;
using DePan.Models;
using System.Security.Claims;

namespace DePan.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly CarritoService _carritoService;
        private readonly ProductoService _productoService;

        public CarritoController(CarritoService carritoService, ProductoService productoService)
        {
            _carritoService = carritoService;
            _productoService = productoService;
        }

        // GET: /Carrito
        public async Task<IActionResult> Index()
        {
            var usuarioId = GetUsuarioId();
            var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);
            
            if (carrito == null)
            {
                carrito = new Carrito { LineaCarritos = new List<LineaCarrito>() };
            }

            // Verificar si hay una notificación de expiración activa
            ViewBag.MostrarNotificacionExpiracion = Request.Cookies["carrito_expirado"] == "true";

            return View(carrito);
        }
        
        // POST: /Carrito/DismissExpiracion
        [HttpPost]
        public IActionResult DismissExpiracion()
        {
            Response.Cookies.Delete("carrito_expirado");
            return Json(new { success = true });
        }
        // GET: /Carrito/GetCantidadItems
        [HttpGet]
        public async Task<IActionResult> GetCantidadItems()
        {
            var usuarioId = GetUsuarioId();
            var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);
            var totalItems = carrito != null ? _carritoService.ObtenerCantidadItems(carrito) : 0;
            
            return Json(new { totalItems });
        }

        // GET: /Carrito/CheckExpiracion
        [HttpGet]
        public async Task<IActionResult> CheckExpiracion()
        {
            var usuarioId = GetUsuarioId();
            var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);
            
            if (carrito == null || !carrito.LineaCarritos.Any())
            {
                return Json(new { tieneAlerta = false });
            }

            // Verificar si hay algún producto con menos de 1 minuto para expirar
            var ahora = DateTime.Now;
            var tiempoExpiracion = TimeSpan.FromMinutes(2); // Debe coincidir con el valor en CarritoCleanupService
            var umbralAlerta = TimeSpan.FromMinutes(1);
            
            var hayProductosProximosExpirar = carrito.LineaCarritos.Any(lc => 
            {
                var tiempoTranscurrido = ahora - lc.FechaReserva;
                var tiempoRestante = tiempoExpiracion - tiempoTranscurrido;
                return tiempoRestante <= umbralAlerta && tiempoRestante > TimeSpan.Zero;
            });

            return Json(new { tieneAlerta = hayProductosProximosExpirar });
        }

        // POST: /Carrito/Agregar
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] object body = null)
        {
            // Try to bind JSON body first (usual for fetch with application/json)
            int productoId = 0;
            int cantidad = 1;

            try
            {
                if (body != null)
                {
                    // Attempt to read common JSON shape: { productoId: x, cantidad: y }
                    // Use System.Text.Json via a simple dynamic parse
                    var json = System.Text.Json.JsonSerializer.Serialize(body);
                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("productoId", out var pIdProp) && pIdProp.TryGetInt32(out var pId))
                    {
                        productoId = pId;
                    }
                    if (root.TryGetProperty("cantidad", out var cantidadProp) && cantidadProp.TryGetInt32(out var c))
                    {
                        cantidad = c;
                    }
                }
            }
            catch
            {
                // Ignore JSON parse errors and fallback below
                productoId = 0;
                cantidad = 1;
            }

            // Fallback: if productoId still 0, try form or querystring (for non-JSON posts)
            if (productoId == 0)
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    if (form.ContainsKey("productoId") && int.TryParse(form["productoId"], out var fPid))
                    {
                        productoId = fPid;
                    }

                    if (form.ContainsKey("cantidad") && int.TryParse(form["cantidad"], out var fCant))
                    {
                        cantidad = fCant;
                    }
                }
                else if (Request.Query.ContainsKey("productoId") && int.TryParse(Request.Query["productoId"], out var qPid))
                {
                    productoId = qPid;
                    if (Request.Query.ContainsKey("cantidad") && int.TryParse(Request.Query["cantidad"], out var qCant))
                    {
                        cantidad = qCant;
                    }
                }
            }

            var usuarioId = GetUsuarioId();
            var producto = await _productoService.GetProductoByIdAsync(productoId);

            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado", productoId });
            }

            if (producto.Stock < cantidad)
            {
                return Json(new { success = false, message = "Stock insuficiente", stock = producto.Stock });
            }

            var result = await _carritoService.AgregarAlCarritoAsync(usuarioId, productoId, cantidad);
            
            if (result)
            {
                var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);
                var totalItems = carrito != null ? _carritoService.ObtenerCantidadItems(carrito) : 0;
                
                return Json(new { 
                    success = true, 
                    message = "Producto agregado al carrito",
                    totalItems = totalItems
                });
            }

            return Json(new { success = false, message = "Error al agregar al carrito" });
        }

        // POST: /Carrito/ActualizarCantidad
        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int lineaCarritoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                return await Eliminar(lineaCarritoId);
            }

            var result = await _carritoService.ActualizarCantidadCarritoAsync(lineaCarritoId, cantidad);
            
            if (result)
            {
                var usuarioId = GetUsuarioId();
                var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);
                var total = carrito != null ? carrito.Total : 0;
                var totalItems = carrito != null ? _carritoService.ObtenerCantidadItems(carrito) : 0;

                return Json(new { 
                    success = true, 
                    total = total.ToString("C"),
                    totalItems = totalItems
                });
            }

            return Json(new { success = false, message = "Error al actualizar cantidad" });
        }

        // POST: /Carrito/Eliminar
        [HttpPost]
        public async Task<IActionResult> Eliminar(int lineaCarritoId)
        {
            var result = await _carritoService.EliminarDelCarritoAsync(lineaCarritoId);
            
            if (result)
            {
                var usuarioId = GetUsuarioId();
                var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);
                var total = carrito != null ? carrito.Total : 0;
                var totalItems = carrito != null ? _carritoService.ObtenerCantidadItems(carrito) : 0;

                return Json(new { 
                    success = true, 
                    total = total.ToString("C"),
                    totalItems = totalItems
                });
            }

            return Json(new { success = false, message = "Error al eliminar del carrito" });
        }

        // POST: /Carrito/Vaciar
        [HttpPost]
        public async Task<IActionResult> Vaciar()
        {
            var usuarioId = GetUsuarioId();
            var result = await _carritoService.VaciarCarritoAsync(usuarioId);
            
            if (result)
            {
                return Json(new { success = true, message = "Carrito vaciado" });
            }

            return Json(new { success = false, message = "Error al vaciar carrito" });
        }

        // Obtener ID del usuario desde el token JWT
        private int GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }
    }
}