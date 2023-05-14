using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOaDAO
{
    public class CatInfoMedicoDTOaDAO
    {
        public static CatInfoMedico catInfoMedicoDTOaDAO(CatInfoMedicoDTO infoMedicoDTO)
        {
            CatInfoMedico infoMedico = new CatInfoMedico();

            infoMedico.Id = infoMedicoDTO.Id;
            infoMedico.NombreMedico = infoMedicoDTO.NombreMedico;
            infoMedico.Especialidad = infoMedicoDTO.Especialidad;
            infoMedico.CodSala = infoMedicoDTO.CodSala;
            infoMedico.CodPlanta = infoMedicoDTO.CodPlanta;
           
            return infoMedico;

        }

        public static List<CatInfoMedico> listaCatInfoMedicoDTOaDAO(List<CatInfoMedicoDTO> listaInfoMedicoDTO)
        {
            List<CatInfoMedico> listaInfoMedico = new List<CatInfoMedico>();


            foreach (var infoMedicoDTO in listaInfoMedicoDTO)
            {
                CatInfoMedico infoMedico = catInfoMedicoDTOaDAO(infoMedicoDTO);


                listaInfoMedico.Add(infoMedico);
            }


            return listaInfoMedico;
        }

    }
}

