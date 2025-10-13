using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class VProductosMasVendido
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string Categoria { get; set; } = null!;

    public long TotalVentas { get; set; }

    public decimal? UnidadesVendidas { get; set; }

    public decimal? IngresosTotales { get; set; }
}
