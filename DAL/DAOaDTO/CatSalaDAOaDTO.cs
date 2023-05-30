using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOaDTO
{
    public class CatSalaDAOaDTO
    {
          public static CatSalaDTO catSalaDAOaDTO(CatSalaCitum sala)
        {
            CatSalaDTO salaDTO = new CatSalaDTO();

            salaDTO.Id = sala.Id;
            salaDTO.CodSala = sala.CodSala;
            salaDTO.NombreSala = sala.NombreSala;

            return salaDTO;

        }

        public static List<CatSalaDTO> listacatSalaDAOaDTO(List<CatSalaCitum> listaSalaCita)
        {
            List<CatSalaDTO> listaSalaDTO = new List<CatSalaDTO>();


            foreach (var sala in listaSalaCita)
            {
                CatSalaDTO salaDTO = catSalaDAOaDTO(sala);


                listaSalaDTO.Add(salaDTO);
            }


            return listaSalaDTO;
        }
    }
}
