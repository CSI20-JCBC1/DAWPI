using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CatEstadoCitum
{
    public long Id { get; set; }

    public string EstadoCita { get; set; } = null!;

    public string? DescEstadoCita { get; set; }
}
