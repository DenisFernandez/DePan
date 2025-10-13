using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int IdUsuarioCliente { get; set; }

    public int? IdRepartidor { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public DateTime FechaPedido { get; set; }

    public decimal Subtotal { get; set; }

    public decimal GastosEnvio { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public string DireccionEntrega { get; set; } = null!;

    public string CiudadEntrega { get; set; } = null!;

    public string CodigoPostalEntrega { get; set; } = null!;

    public string TelefonoContacto { get; set; } = null!;

    public string? Notas { get; set; }

    public DateTime? FechaEntregaEstimada { get; set; }

    public DateTime? FechaEntregaReal { get; set; }

    public virtual Usuario? IdRepartidorNavigation { get; set; }

    public virtual Usuario IdUsuarioClienteNavigation { get; set; } = null!;

    public virtual ICollection<LineaPedido> LineaPedidos { get; set; } = new List<LineaPedido>();

    public virtual ICollection<SeguimientoPedido> SeguimientoPedidos { get; set; } = new List<SeguimientoPedido>();
}
