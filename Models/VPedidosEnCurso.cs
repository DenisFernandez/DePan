using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class VPedidosEnCurso
{
    public int IdPedido { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime FechaPedido { get; set; }

    public string Cliente { get; set; } = null!;

    public string? Repartidor { get; set; }

    /// <summary>
    /// Ubicación del repartidor
    /// </summary>
    public decimal? Latitud { get; set; }

    /// <summary>
    /// Ubicación del repartidor
    /// </summary>
    public decimal? Longitud { get; set; }

    public DateTime? UltimaActualizacion { get; set; }
}
