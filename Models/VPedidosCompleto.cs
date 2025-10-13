using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class VPedidosCompleto
{
    public int IdPedido { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public DateTime FechaPedido { get; set; }

    public string Estado { get; set; } = null!;

    public decimal Total { get; set; }

    public string ClienteNombre { get; set; } = null!;

    public string ClienteApellidos { get; set; } = null!;

    public string ClienteEmail { get; set; } = null!;

    public string? ClienteTelefono { get; set; }

    public string DireccionEntrega { get; set; } = null!;

    public string? RepartidorNombre { get; set; }

    public string? RepartidorApellidos { get; set; }
}
