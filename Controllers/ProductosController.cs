using Microsoft.AspNetCore.Mvc;
using DePan.Models;
using DePan.Services;
using Microsoft.AspNetCore.Authorization;

namespace DePan.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoService _productoService;

        public ProductosController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: /Productos - Catálogo público
        public async Task<IActionResult> Index(int? categoriaId, string? search)
        {
            ViewBag.Categorias = await _productoService.GetCategoriasAsync();
            
            List<Producto> productos;

            if (!string.IsNullOrEmpty(search))
            {
                productos = await _productoService.SearchProductosAsync(search);
                ViewBag.SearchTerm = search;
            }
            else if (categoriaId.HasValue)
            {
                productos = await _productoService.GetProductosByCategoriaAsync(categoriaId.Value);
                ViewBag.CategoriaSeleccionada = categoriaId.Value;
            }
            else
            {
                productos = await _productoService.GetAllProductosAsync();
            }

            return View(productos);
        }

        // GET: /Productos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // ========== SECCIÓN ADMINISTRADOR ==========
        
        // GET: /Productos/Admin
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Admin()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return View(productos);
        }

        // GET: /Productos/Create
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            var categorias = await _productoService.GetCategoriasAsync();
            var model = new ProductoViewModel
            {
                Categorias = categorias
            };
            return View(model);
        }

        // POST: /Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(ProductoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var producto = new Producto
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Precio = model.Precio,
                    IdCategoria = model.IdCategoria,
                    ImagenUrl = model.ImagenUrl,
                    Stock = model.Stock,
                    Disponible = model.Disponible
                };

                var result = await _productoService.CreateProductoAsync(producto);
                if (result)
                {
                    return RedirectToAction(nameof(Admin));
                }
                ModelState.AddModelError("", "Error al crear el producto");
            }

            // Recargar categorías si hay error
            model.Categorias = await _productoService.GetCategoriasAsync();
            return View(model);
        }

        // GET: /Productos/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            var categorias = await _productoService.GetCategoriasAsync();
            var model = new ProductoViewModel
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                IdCategoria = producto.IdCategoria,
                ImagenUrl = producto.ImagenUrl,
                Stock = producto.Stock,
                Disponible = producto.Disponible ?? true,
                Categorias = categorias,
                NombreCategoria = producto.IdCategoriaNavigation?.Nombre
            };

            return View(model);
        }

        // POST: /Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, ProductoViewModel model)
        {
            if (id != model.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var producto = await _productoService.GetProductoByIdAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }

                producto.Nombre = model.Nombre;
                producto.Descripcion = model.Descripcion;
                producto.Precio = model.Precio;
                producto.IdCategoria = model.IdCategoria;
                producto.ImagenUrl = model.ImagenUrl;
                producto.Stock = model.Stock;
                producto.Disponible = model.Disponible;

                var result = await _productoService.UpdateProductoAsync(producto);
                if (result)
                {
                    return RedirectToAction(nameof(Admin));
                }
                ModelState.AddModelError("", "Error al actualizar el producto");
            }

            // Recargar categorías si hay error
            model.Categorias = await _productoService.GetCategoriasAsync();
            return View(model);
        }

        // POST: /Productos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productoService.DeleteProductoAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Admin));
            }
            return NotFound();
        }
    }
}