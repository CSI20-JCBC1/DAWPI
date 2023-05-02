using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOaDTO
{
    public class CitaDAOaDTO
    {
        public static CitaDTO citaDAOaDTO(Cita cita)
        {

           CitaDTO citaDTO = new CitaDTO();
            citaDTO.Id = cita.Id;
            citaDTO.MdUuid = cita.MdUuid;
            citaDTO.MdDate = cita.MdDate;
            citaDTO.Id = cita.Id;
            citaDTO.Asunto = cita.Asunto;
            citaDTO.NombrePaciente = cita.NombrePaciente;
            citaDTO.Fecha = cita.Fecha;
            citaDTO.Sintomas = cita.Sintomas;
            citaDTO.NombreMedico = cita.NombreMedico;
            citaDTO.Hora = cita.Hora;
            citaDTO.CodPlanta = cita.CodPlanta;
            citaDTO.CodSala = cita.CodSala;
            citaDTO.Enfermedad = cita.Enfermedad;
            citaDTO.Solucion = cita.Solucion;
            citaDTO.EstadoCita = cita.EstadoCita;

            return citaDTO;
        }

        public static List<CitaDTO> listaCitaDAOaDTO(List<Cita> listaCita)
        {

            List<CitaDTO> listaCitaDTO = new List<CitaDTO>();


            foreach (var cita in listaCita)
            {
                CitaDTO citaDTO = citaDAOaDTO(cita);


                listaCitaDTO.Add(citaDTO);
            }


            return listaCitaDTO;
        }
    }
}

