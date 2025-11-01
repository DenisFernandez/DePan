using System.ComponentModel.DataAnnotations;

namespace DePan.Models
{
    public class CheckoutViewModel
    {
        public Carrito? Carrito { get; set; }

        [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string DireccionEntrega { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
        public string CiudadEntrega { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [StringLength(10, ErrorMessage = "El código postal no puede exceder 10 caracteres")]
        public string CodigoPostalEntrega { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono de contacto es obligatorio")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string TelefonoContacto { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Las notas no pueden exceder 500 caracteres")]
        public string? Notas { get; set; }
    }
}