using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public string? ImagenUrl { get; set; }

    public int Stock { get; set; }

    public bool? Disponible { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<LineaCarrito> LineaCarritos { get; set; } = new List<LineaCarrito>();

    public virtual ICollection<LineaPedido> LineaPedidos { get; set; } = new List<LineaPedido>();

    public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
}
