using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CatInfoMedicoDTO
    {
        //Propiedades
        private long id;

        private string nombreMedico;

        private string? especialidad;

        //Getters & Setters
        public long Id { get => id; set => id = value; }
        public string NombreMedico { get => nombreMedico; set => nombreMedico = value; }
        public string? Especialidad { get => especialidad; set => especialidad = value; }

        //Constructores
        public CatInfoMedicoDTO(long id, string nombreMedico, string? especialidad)
        {
            this.Id = id;
            this.NombreMedico = nombreMedico;
            this.Especialidad = especialidad;
        }
        public CatInfoMedicoDTO(string nombreMedico, string? especialidad)
        {
            this.NombreMedico = nombreMedico;
            this.Especialidad = especialidad;
        }
        public CatInfoMedicoDTO()
        {
        }
    }
}
