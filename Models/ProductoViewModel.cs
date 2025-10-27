using System.ComponentModel.DataAnnotations;

namespace DePan.Models
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        public int IdCategoria { get; set; }

        [Url(ErrorMessage = "La URL de la imagen debe ser válida")]
        public string? ImagenUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public bool Disponible { get; set; } = true;

        // Para mostrar en las vistas
        public string? NombreCategoria { get; set; }
        public List<Categorium>? Categorias { get; set; }
    }
}