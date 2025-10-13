using Microsoft.AspNetCore.Mvc;
using DePan.Data;

namespace DePan.Controllers
{
    public class TestDbController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestDbController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                ViewBag.CanConnect = canConnect;
                
                // Si tienes una tabla Usuarios, prueba contar registros
                // var userCount = _context.Usuarios?.Count() ?? 0;
                // ViewBag.UserCount = userCount;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}