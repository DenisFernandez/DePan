using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DePan.Services;
using DePan.Models;
using System.Security.Claims;

namespace DePan.Controllers
{
    // DTO para agregar al carrito
    public class AgregarAlCarritoRequest
    {
        public int productoId { get; set; }
        public int cantidad { get; set; }
    }

    // DTO para eliminar línea de carrito
    public class EliminarLineaCarritoRequest
    {
        public int lineaCarritoId { get; set; }
    }

    // DTO para actualizar cantidad
    public class ActualizarCantidadRequest
    {
        public int lineaCarritoId { get; set; }
        public int cantidad { get; set; }
    }

    public class CarritoController : Controller
    {
        private readonly CarritoService _carritoService;
        private readonly ProductoService _productoService;

        public CarritoController(CarritoService carritoService, ProductoService productoService)
        {
            _carritoService = carritoService;
            _productoService = productoService;
        }

        // Método helper para verificar autenticación
        private bool VerificarAutenticacion()
        {
            return User?.Identity?.IsAuthenticated ?? false;
        }

        // GET: /Carrito
        public async Task<IActionResult> Index()
        {
            // Verificar si el usuario está autenticado
            if (!VerificarAutenticacion())
            {
                TempData["MensajeError"] = "Debes iniciar sesión para acceder a tu carrito.";
                return RedirectToAction("Login", "Auth");
            }

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
        public async Task<IActionResult> Agregar([FromBody] AgregarAlCarritoRequest request)
        {
            // Verificar si el usuario está autenticado
            if (!VerificarAutenticacion())
            {
                return Json(new { 
                    success = false, 
                    message = "Debes iniciar sesión para agregar productos al carrito",
                    requiresLogin = true
                });
            }

            // Validar que los parámetros no sean nulos o inválidos
            if (request == null || request.productoId <= 0)
            {
                return Json(new { success = false, message = "ID de producto inválido", productoId = request?.productoId ?? 0 });
            }

            int cantidad = request.cantidad > 0 ? request.cantidad : 1;

            var usuarioId = GetUsuarioId();
            var producto = await _productoService.GetProductoByIdAsync(request.productoId);

            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado", productoId = request.productoId });
            }

            if (producto.Stock < cantidad)
            {
                return Json(new { success = false, message = "Stock insuficiente", stock = producto.Stock });
            }

            var result = await _carritoService.AgregarAlCarritoAsync(usuarioId, request.productoId, cantidad);
            
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
        public async Task<IActionResult> ActualizarCantidad([FromBody] ActualizarCantidadRequest request)
        {
            if (request == null || request.cantidad <= 0)
            {
                return Json(new { success = false, message = "La cantidad debe ser mayor a 0" });
            }

            var result = await _carritoService.ActualizarCantidadCarritoAsync(request.lineaCarritoId, request.cantidad);
            
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
        public async Task<IActionResult> Eliminar([FromBody] EliminarLineaCarritoRequest request)
        {
            if (request == null || request.lineaCarritoId <= 0)
            {
                return Json(new { success = false, message = "ID de línea de carrito inválido" });
            }

            var result = await _carritoService.EliminarDelCarritoAsync(request.lineaCarritoId);
            
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