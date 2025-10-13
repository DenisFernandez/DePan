using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class SeguimientoPedido
{
    public int IdSeguimiento { get; set; }

    public int IdPedido { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaEstado { get; set; }

    public string? Observaciones { get; set; }

    /// <summary>
    /// Ubicación del repartidor
    /// </summary>
    public decimal? Latitud { get; set; }

    /// <summary>
    /// Ubicación del repartidor
    /// </summary>
    public decimal? Longitud { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
