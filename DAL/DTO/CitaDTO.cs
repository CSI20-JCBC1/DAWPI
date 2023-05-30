using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CitaDTO
    {

        //Propiedades
        private string? mdUuid;

        private DateTime? mdDate;

        private int id;

        private string? asunto;

        private string? nombrePaciente;

        private DateOnly? fecha;

        private string? sintomas;

        private string? nombreMedico;

        private TimeOnly? hora;

        private string? codPlanta;

        private string? codSala;

        private string? enfermedad;

        private string? solucion;

        private string? estadoCita;


        //Constructores
        public CitaDTO(int id, string asunto, string nombrePaciente, string sintomas, DateOnly fecha, TimeOnly hora, string nombreMedico, string codPlanta, string codSala, string enfermedad, string solucion, string estadoCita)
        {
            this.MdUuid = Guid.NewGuid().ToString();
            this.MdDate = DateTime.Now;
            this.Id = id;
            this.Asunto = asunto;
            this.NombrePaciente = nombrePaciente; 
            this.Sintomas = sintomas;
            this.Fecha = fecha;
            this.NombreMedico = nombreMedico;
            this.Hora = hora;
            this.CodPlanta = codPlanta;
            this.CodSala = codSala;
            this.Enfermedad = enfermedad;
            this.Solucion = solucion;
            this.EstadoCita = estadoCita;
        }

        public CitaDTO(string asunto, string nombrePaciente, string sintomas, DateOnly fecha, TimeOnly hora, string nombreMedico, string codPlanta, string codSala, string enfermedad, string solucion, string estadoCita)
        {
            this.MdUuid = Guid.NewGuid().ToString();
            this.MdDate = DateTime.Now;
            this.Asunto = asunto;
            this.NombrePaciente = nombrePaciente;
            this.Fecha = fecha;
            this.Sintomas = sintomas;
            this.NombreMedico = nombreMedico;
            this.Hora = hora;
            this.CodPlanta = codPlanta;
            this.CodSala = codSala;
            this.Enfermedad = enfermedad;
            this.Solucion = solucion;
            this.EstadoCita = estadoCita;
        }

        public CitaDTO(string asunto, string nombrePaciente, string sintomas)
        {
            this.MdUuid = Guid.NewGuid().ToString();
            this.MdDate = DateTime.Now;
            this.Id = id;
            this.Asunto = asunto;
            this.NombrePaciente = nombrePaciente;
            this.Sintomas = sintomas;
            this.EstadoCita = "PA";
        }

        public CitaDTO( )
        {
            
        }

        //Getters & Setters
        public string? MdUuid { get => mdUuid; set => mdUuid = value; }
        public DateTime? MdDate { get => mdDate; set => mdDate = value; }
        public int Id { get => id; set => id = value; }
        public string? Asunto { get => asunto; set => asunto = value; }
        public string? NombrePaciente { get => nombrePaciente; set => nombrePaciente = value; }
        public DateOnly? Fecha { get => fecha; set => fecha = value; }
        public string? Sintomas { get => sintomas; set => sintomas = value; }
        public string? NombreMedico { get => nombreMedico; set => nombreMedico = value; }
        public TimeOnly? Hora { get => hora; set => hora = value; }
        public string? CodPlanta { get => codPlanta; set => codPlanta = value; }
        public string? CodSala { get => codSala; set => codSala = value; }
        public string? Enfermedad { get => enfermedad; set => enfermedad = value; }
        public string? Solucion { get => solucion; set => solucion = value; }
        public string? EstadoCita { get => estadoCita; set => estadoCita = value; }



    }
}
