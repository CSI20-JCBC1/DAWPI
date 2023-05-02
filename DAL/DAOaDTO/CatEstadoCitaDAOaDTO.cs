using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOaDTO
{
    public class CatEstadoCitaDAOaDTO
    {
        public static CatEstadoCitaDTO catEstadoCitaDAOaDTO(CatEstadoCitum estadoCita)
        {
            CatEstadoCitaDTO estadoCitaDTO = new CatEstadoCitaDTO();

            estadoCitaDTO.Id = estadoCita.Id;
            estadoCitaDTO.EstadoCita = estadoCita.EstadoCita;
            estadoCitaDTO.DescEstadoCita= estadoCita.DescEstadoCita;

            return estadoCitaDTO;

        }

        public static List<CatEstadoCitaDTO> listaCatEstadoCitaDAOaDTO (List<CatEstadoCitum> listaEstadoCita)
        {
            List<CatEstadoCitaDTO> listaEstadoCitaDTO = new List<CatEstadoCitaDTO>();


            foreach (var estadoCita in listaEstadoCita)
            {
                CatEstadoCitaDTO estadoCitaDTO = catEstadoCitaDAOaDTO(estadoCita);


                listaEstadoCitaDTO.Add(estadoCitaDTO);
            }


            return listaEstadoCitaDTO;
        }
    }
}
