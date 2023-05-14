using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOaDTO
{
    public class CatInfoMedicoDAOaDTO
    {
        public static CatInfoMedicoDTO catInfoMedicoDAOaDTO(CatInfoMedico infoMedico)
        {
            CatInfoMedicoDTO infoMedicoDTO = new CatInfoMedicoDTO();

            infoMedicoDTO.Id = infoMedico.Id;
            infoMedicoDTO.NombreMedico = infoMedico.NombreMedico;
            infoMedicoDTO.Especialidad = infoMedico.Especialidad;
            infoMedicoDTO.CodSala = infoMedico.CodSala;
            infoMedicoDTO.CodPlanta = infoMedico.CodPlanta;

            return infoMedicoDTO;

        }

        public static List<CatInfoMedicoDTO> listaCatInfoMedicoDAOaDTO(List<CatInfoMedico> listaInfoMedico)
        {
            List<CatInfoMedicoDTO> listaInfoMedicoDTO = new List<CatInfoMedicoDTO>();


            foreach (var infoMedico in listaInfoMedico)
            {
                CatInfoMedicoDTO infoMedicoDTO = catInfoMedicoDAOaDTO(infoMedico);


                listaInfoMedicoDTO.Add(infoMedicoDTO);
            }


            return listaInfoMedicoDTO;
        }

    }
}
