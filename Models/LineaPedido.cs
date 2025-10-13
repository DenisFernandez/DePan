using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class LineaPedido
{
    public int IdLineaPedido { get; set; }

    public int IdPedido { get; set; }

    public int IdProducto { get; set; }

    /// <summary>
    /// Snapshot del nombre en el momento del pedido
    /// </summary>
    public string NombreProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
