using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CatPlantaDTO
    {
        //Propiedades
        private long id;

        private string codPlanta;

        private string? nombrePlanta;
        //Getters & Setters

        public long Id { get => id; set => id = value; }
        public string CodPlanta { get => codPlanta; set => codPlanta = value; }
        public string? NombrePlanta { get => nombrePlanta; set => nombrePlanta = value; }

        //Constructores
        public CatPlantaDTO() { }
        public CatPlantaDTO(long id, string codPlanta, string? nombrePlanta)
        {
            this.Id = id;
            this.CodPlanta = codPlanta;
            this.NombrePlanta = nombrePlanta;
        }
    }
}
