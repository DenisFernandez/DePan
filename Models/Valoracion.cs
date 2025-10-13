using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class Valoracion
{
    public int IdValoracion { get; set; }

    public int IdUsuario { get; set; }

    public int IdProducto { get; set; }

    public int Puntuacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime FechaValoracion { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
