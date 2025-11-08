using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DePan.Models;

public partial class LineaCarrito
{
    public int IdLineaCarrito { get; set; }

    public int IdCarrito { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    [Column("fecha_reserva")]
    public DateTime FechaReserva { get; set; }

    public virtual Carrito IdCarritoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
