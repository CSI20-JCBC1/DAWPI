using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOaDAO
{
    public class CitaDTOaDAO
    {
        public static Cita citaDTOaDAO(CitaDTO citaDTO)
        {

            Cita cita = new Cita();
            cita.MdUuid = citaDTO.MdUuid;
            cita.MdDate = citaDTO.MdDate;
            cita.Id = citaDTO.Id;
            cita.Asunto = citaDTO.Asunto;
            cita.NombrePaciente = citaDTO.NombrePaciente;
            cita.Fecha = citaDTO.Fecha;
            cita.Sintomas = citaDTO.Sintomas;
            cita.NombreMedico = citaDTO.NombreMedico;
            cita.Hora = citaDTO.Hora;
            cita.CodPlanta = citaDTO.CodPlanta;
            cita.CodSala = citaDTO.CodSala;
            cita.Enfermedad = citaDTO.Enfermedad;
            cita.Solucion = citaDTO.Solucion;
            cita.EstadoCita = citaDTO.EstadoCita;

            return cita;
        }
    }
}
