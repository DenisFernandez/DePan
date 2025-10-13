using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public decimal Total { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<LineaCarrito> LineaCarritos { get; set; } = new List<LineaCarrito>();
}
