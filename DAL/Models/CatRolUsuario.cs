using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CatRolUsuario
{
    public long Id { get; set; }

    public string ControlAcceso { get; set; } = null!;

    public long NivelAcceso { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
