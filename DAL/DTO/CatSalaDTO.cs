using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CatSalaDTO
    {
        //Propiedades
        private long id;

        private string codSala;

        private string? nombreSala;

        //Getters & Setters
        public long Id { get => id; set => id = value; }
        public string CodSala { get => codSala; set => codSala = value; }
        public string? NombreSala { get => nombreSala; set => nombreSala = value; }

        //Constructores
        public CatSalaDTO()
        {
        }

        public CatSalaDTO(long id, string codSala, string? nombreSala)
        {
            this.Id = id;
            this.CodSala = codSala;
            this.NombreSala = nombreSala;
        }


    }
}
