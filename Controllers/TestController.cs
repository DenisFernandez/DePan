using Microsoft.AspNetCore.Mvc;

namespace DePan.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return Content("<h1>Test Controller funciona</h1><p><a href='/Auth/Login'>Ir a Login</a></p>", "text/html");
        }

        public IActionResult AuthTest()
        {
            return Content(@"
                <html>
                <head>
                    <title>Test Auth</title>
                    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'>
                </head>
                <body>
                    <div class='container mt-5'>
                        <h1>Test de Autenticación</h1>
                        <div class='d-grid gap-2 col-6 mx-auto'>
                            <a href='/Auth/Login' class='btn btn-primary'>Ir a Login</a>
                            <a href='/Auth/Register' class='btn btn-success'>Ir a Register</a>
                            <a href='/Home/Index' class='btn btn-secondary'>Ir a Home</a>
                        </div>
                    </div>
                </body>
                </html>
            ", "text/html");
        }
    }
}