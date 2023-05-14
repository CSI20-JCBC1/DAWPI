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

        private string codSala;

        private string codPlanta;

        //Getters & Setters
        public long Id { get => id; set => id = value; }
        public string NombreMedico { get => nombreMedico; set => nombreMedico = value; }
        public string? Especialidad { get => especialidad; set => especialidad = value; }
        public string CodSala { get => codSala; set => codSala = value; }
        public string CodPlanta { get => codPlanta; set => codPlanta = value; }

        //Constructores
        public CatInfoMedicoDTO(long id, string nombreMedico, string? especialidad, string codSala, string codPlanta)
        {
            this.id = id;
            this.nombreMedico = nombreMedico;
            this.especialidad = especialidad;
            this.codSala = codSala;
            this.codPlanta = codPlanta;
        }
        public CatInfoMedicoDTO(string nombreMedico, string? especialidad, string codSala, string codPlanta)
        {
            this.NombreMedico = nombreMedico;
            this.Especialidad = especialidad;
            this.codSala = codSala;
            this.codPlanta = codPlanta;
        }
        public CatInfoMedicoDTO()
        {
        }

        
    }
}
