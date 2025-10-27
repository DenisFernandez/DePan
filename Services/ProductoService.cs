using Microsoft.EntityFrameworkCore;
using DePan.Data;
using DePan.Models;

namespace DePan.Services
{
    public class ProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los productos con categoría usando LINQ e Include
        public async Task<List<Producto>> GetAllProductosAsync()
        {
            return await _context.Productos
                .Include(p => p.IdCategoriaNavigation) // Incluir la categoría
                .Where(p => p.Disponible == true)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        // Obtener productos por categoría ID con LINQ
        public async Task<List<Producto>> GetProductosByCategoriaAsync(int categoriaId)
        {
            return await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Where(p => p.IdCategoria == categoriaId && p.Disponible == true)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        // Buscar productos con LINQ
        public async Task<List<Producto>> SearchProductosAsync(string searchTerm)
        {
            return await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Where(p => p.Disponible == true && 
                           (p.Nombre.Contains(searchTerm) || 
                            (p.Descripcion != null && p.Descripcion.Contains(searchTerm))))
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        // Obtener producto por ID con categoría
        public async Task<Producto?> GetProductoByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(p => p.IdProducto == id && p.Disponible == true);
        }

        // Crear nuevo producto
        public async Task<bool> CreateProductoAsync(Producto producto)
        {
            try
            {
                producto.FechaCreacion = DateTime.Now;
                producto.FechaActualizacion = DateTime.Now;
                producto.Disponible = true;
                
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Actualizar producto
        public async Task<bool> UpdateProductoAsync(Producto producto)
        {
            try
            {
                producto.FechaActualizacion = DateTime.Now;
                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Eliminar producto (soft delete)
        public async Task<bool> DeleteProductoAsync(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto != null)
                {
                    producto.Disponible = false;
                    producto.FechaActualizacion = DateTime.Now;
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

        // Obtener todas las categorías con LINQ
        public async Task<List<Categorium>> GetCategoriasAsync()
        {
            return await _context.Categoria
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        // Obtener categoría por ID
        public async Task<Categorium?> GetCategoriaByIdAsync(int id)
        {
            return await _context.Categoria
                .FirstOrDefaultAsync(c => c.IdCategoria == id);
        }
    }
}