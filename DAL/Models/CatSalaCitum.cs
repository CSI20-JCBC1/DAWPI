using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CatSalaCitum
{
    public long Id { get; set; }

    public string CodSala { get; set; } = null!;

    public string? NombreSala { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
