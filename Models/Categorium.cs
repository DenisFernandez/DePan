using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
