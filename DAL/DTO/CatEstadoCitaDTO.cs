using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CatEstadoCitaDTO
    {
        //Propiedades
        private long id;

        private string estadoCita;

        private string? descEstadoCita;

        //Getters & Setters

        public long Id { get => id; set => id = value; }
        public string EstadoCita { get => estadoCita; set => estadoCita = value; }
        public string? DescEstadoCita { get => descEstadoCita; set => descEstadoCita = value; }

        //Constructores
        public CatEstadoCitaDTO(long id, string estadoCita, string? descEstadoCita)
        {
            this.id = id;
            this.estadoCita = estadoCita;
            this.descEstadoCita = descEstadoCita;
        }

        public CatEstadoCitaDTO(string estadoCita, string? descEstadoCita)
        {
            this.estadoCita = estadoCita;
            this.descEstadoCita = descEstadoCita;
        }

        public CatEstadoCitaDTO() { }
        
    }
}
