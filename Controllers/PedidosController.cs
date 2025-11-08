using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DePan.Services;
using DePan.Models;
using System.Security.Claims;

namespace DePan.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly PedidoService _pedidoService;
        private readonly CarritoService _carritoService;

        public PedidosController(PedidoService pedidoService, CarritoService carritoService)
        {
            _pedidoService = pedidoService;
            _carritoService = carritoService;
        }

        // GET: /Pedidos/MisPedidos
        public async Task<IActionResult> MisPedidos()
        {
            var usuarioId = GetUsuarioId();
            var pedidos = await _pedidoService.GetPedidosUsuarioAsync(usuarioId);
            return View(pedidos);
        }

        // GET: /Pedidos/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var pedido = await _pedidoService.GetPedidoByIdAsync(id);
            
            if (pedido == null)
            {
                return NotFound();
            }

            // Verificar que el pedido pertenece al usuario actual (a menos que sea admin)
            var usuarioId = GetUsuarioId();
            if (pedido.IdUsuarioCliente != usuarioId && !User.IsInRole("Administrador") && !User.IsInRole("administrador"))
            {
                return Forbid();
            }

            return View(pedido);
        }

        // GET: /Pedidos/Checkout
        public async Task<IActionResult> Checkout()
        {
            var usuarioId = GetUsuarioId();
            var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);

            if (carrito == null || !carrito.LineaCarritos.Any())
            {
                return RedirectToAction("Index", "Carrito");
            }

            var viewModel = new CheckoutViewModel
            {
                Carrito = carrito,
                DireccionEntrega = "",
                CiudadEntrega = "",
                CodigoPostalEntrega = "",
                TelefonoContacto = "",
                Notas = ""
            };

            return View(viewModel);
        }

        // POST: /Pedidos/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var usuarioId = GetUsuarioId();
            var carrito = await _carritoService.GetCarritoUsuarioAsync(usuarioId);

            if (carrito == null || !carrito.LineaCarritos.Any())
            {
                ModelState.AddModelError("", "El carrito está vacío");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var pedido = await _pedidoService.CrearPedidoDesdeCarritoAsync(
                    usuarioId,
                    model.DireccionEntrega,
                    model.CiudadEntrega,
                    model.CodigoPostalEntrega,
                    model.TelefonoContacto,
                    model.Notas
                );

                if (pedido != null)
                {
                    return RedirectToAction("Confirmacion", new { id = pedido.IdPedido });
                }

                ModelState.AddModelError("", "Error al crear el pedido");
            }

            model.Carrito = carrito;
            return View(model);
        }

        // GET: /Pedidos/Confirmacion/5
        public async Task<IActionResult> Confirmacion(int id)
        {
            var pedido = await _pedidoService.GetPedidoByIdAsync(id);
            
            if (pedido == null)
            {
                return NotFound();
            }

            // Verificar que el pedido pertenece al usuario actual
            var usuarioId = GetUsuarioId();
            if (pedido.IdUsuarioCliente != usuarioId)
            {
                return Forbid();
            }

            return View(pedido);
        }

        // ========== SECCIÓN ADMINISTRADOR ==========

        // GET: /Pedidos/Admin
        [Authorize(Roles = "Administrador,administrador")]
        public async Task<IActionResult> Admin()
        {
            var pedidos = await _pedidoService.GetAllPedidosAsync();
            var estadisticas = await _pedidoService.GetEstadisticasPedidosAsync();
            
            ViewBag.Estadisticas = estadisticas;
            return View(pedidos);
        }

        // POST: /Pedidos/ActualizarEstado
        [HttpPost]
        [Authorize(Roles = "Administrador,administrador")]
        public async Task<IActionResult> ActualizarEstado([FromBody] ActualizarEstadoRequest request)
        {
            if (request == null || request.PedidoId <= 0 || string.IsNullOrEmpty(request.NuevoEstado))
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            var result = await _pedidoService.ActualizarEstadoPedidoAsync(request.PedidoId, request.NuevoEstado, request.RepartidorId);
            
            if (result)
            {
                return Json(new { success = true, message = "Estado actualizado correctamente" });
            }

            return Json(new { success = false, message = "Error al actualizar estado" });
        }

        // POST: /Pedidos/Eliminar
        [HttpPost]
        [Authorize(Roles = "Administrador,administrador")]
        public async Task<IActionResult> Eliminar([FromBody] EliminarPedidoRequest request)
        {
            if (request == null || request.PedidoId <= 0)
            {
                return Json(new { success = false, message = "ID de pedido inválido" });
            }

            var result = await _pedidoService.EliminarPedidoAsync(request.PedidoId);
            
            if (result)
            {
                return Json(new { success = true, message = "Pedido eliminado correctamente" });
            }

            return Json(new { success = false, message = "Error al eliminar el pedido" });
        }

        // Clase helper para recibir datos JSON
        public class ActualizarEstadoRequest
        {
            public int PedidoId { get; set; }
            public string NuevoEstado { get; set; } = string.Empty;
            public int? RepartidorId { get; set; }
        }

        public class EliminarPedidoRequest
        {
            public int PedidoId { get; set; }
        }

        // GET: /Pedidos/PorEstado
        [Authorize(Roles = "Administrador,administrador")]
        public async Task<IActionResult> PorEstado(string estado)
        {
            var pedidos = await _pedidoService.GetPedidosPorEstadoAsync(estado);
            ViewBag.EstadoFiltro = estado;
            return View("Admin", pedidos);
        }

        // Obtener ID del usuario desde el token JWT
        private int GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }
    }
}