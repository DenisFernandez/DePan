using System;
using System.Collections.Generic;

namespace DePan.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Ciudad { get; set; }

    public string? CodigoPostal { get; set; }

    public string Rol { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual Carrito? Carrito { get; set; }

    public virtual ICollection<Pedido> PedidoIdRepartidorNavigations { get; set; } = new List<Pedido>();

    public virtual ICollection<Pedido> PedidoIdUsuarioClienteNavigations { get; set; } = new List<Pedido>();

    public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
}
