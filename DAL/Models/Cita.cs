using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Cita
{
    public string? MdUuid { get; set; }

    public DateTime? MdDate { get; set; }

    public int Id { get; set; }

    public string? Asunto { get; set; }

    public string? NombrePaciente { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Sintomas { get; set; }

    public string? NombreMedico { get; set; }

    public TimeOnly? Hora { get; set; }

    public string? CodPlanta { get; set; }

    public string? CodSala { get; set; }

    public string? Enfermedad { get; set; }

    public string? Solucion { get; set; }

    public string? EstadoCita { get; set; }

    public virtual CatPlantaCitum? CodPlantaNavigation { get; set; }

    public virtual CatSalaCitum? CodSalaNavigation { get; set; }

    public virtual CatInfoMedico? NombreMedico1 { get; set; }

    public virtual Usuario? NombreMedicoNavigation { get; set; }

    public virtual Usuario? NombrePacienteNavigation { get; set; }
}
