using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class RolDTO
    {
        //Propiedades
        private long id;

        private string controlAcceso;

        private long nivelAcceso;

        //Constructores
        public RolDTO() { }

        public RolDTO(long id, string controlAcceso, long nivelAcceso)
        {
            this.id = id;
            this.controlAcceso = controlAcceso;
            this.nivelAcceso = nivelAcceso;
        }

        //Getters & Setters
        public long Id { get => id; set => id = value; }
        public string ControlAcceso { get => controlAcceso; set => controlAcceso = value; }
        public long NivelAcceso { get => nivelAcceso; set => nivelAcceso = value; }
    }
}
