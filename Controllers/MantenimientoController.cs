using Microsoft.AspNetCore.Mvc;
using DePan.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DePan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador,administrador")]
    public class MantenimientoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MantenimientoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("fix-disponible")]
        public async Task<IActionResult> FixDisponible()
        {
            try
            {
                // Marcar todos los productos con stock > 0 como disponibles
                var productosAActualizar = await _context.Productos
                    .Where(p => p.Stock > 0 && p.Disponible == false)
                    .ToListAsync();

                foreach (var producto in productosAActualizar)
                {
                    producto.Disponible = true;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = $"{productosAActualizar.Count} productos actualizados" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
