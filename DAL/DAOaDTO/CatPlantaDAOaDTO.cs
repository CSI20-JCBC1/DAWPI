using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOaDTO
{
    public class CatPlantaDAOaDTO
    {
        public static CatPlantaDTO catPlantaDAOaDTO(CatPlantaCitum planta)
        {
            CatPlantaDTO plantaDTO = new CatPlantaDTO();

            plantaDTO.Id = planta.Id;
            plantaDTO.CodPlanta = planta.CodPlanta;
            plantaDTO.NombrePlanta = planta.NombrePlanta;

            return plantaDTO;

        }

        public static List<CatPlantaDTO> listacatPlantaDAOaDTO(List<CatPlantaCitum> listaPlantaCita)
        {
            List<CatPlantaDTO> listaPlantaDTO = new List<CatPlantaDTO>();


            foreach (var planta in listaPlantaCita)
            {
                CatPlantaDTO plantaDTO = catPlantaDAOaDTO(planta);


                listaPlantaDTO.Add(plantaDTO);
            }


            return listaPlantaDTO;
        }
    }
}
