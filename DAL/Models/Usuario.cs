using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Usuario
{
    public string MdUuid { get; set; } = null!;

    public DateTime MdDate { get; set; }

    public long Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Movil { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasenya { get; set; } = null!;

    public long Rol { get; set; }

    public virtual ICollection<Cita> CitaNombreMedicoNavigations { get; } = new List<Cita>();

    public virtual ICollection<Cita> CitaNombrePacienteNavigations { get; } = new List<Cita>();

    public virtual CatRolUsuario RolNavigation { get; set; } = null!;
}
