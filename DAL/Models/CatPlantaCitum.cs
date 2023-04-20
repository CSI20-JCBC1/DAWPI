using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CatPlantaCitum
{
    public long Id { get; set; }

    public string CodPlanta { get; set; } = null!;

    public string? NombrePlanta { get; set; }

    public virtual ICollection<Cita> Cita { get; } = new List<Cita>();
}
