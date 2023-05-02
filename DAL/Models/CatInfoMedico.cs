using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CatInfoMedico
{
    public long Id { get; set; }

    public string NombreMedico { get; set; } = null!;

    public string? Especialidad { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
