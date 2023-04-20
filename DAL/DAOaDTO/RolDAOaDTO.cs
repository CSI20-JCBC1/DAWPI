using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOaDTO
{
    public class RolDAOaDTO
    {
        public static RolDTO rolDAOaDTO(CatRolUsuario rol)
        {
            RolDTO rolDTO = new RolDTO();
            rolDTO.Id = rol.Id;
            rolDTO.NivelAcceso = rol.NivelAcceso;
            rolDTO.ControlAcceso= rol.ControlAcceso;

            return rolDTO;
        }

        public static List<RolDTO> listaRolDAOaDTO(List<CatRolUsuario> listaRol)
        {

            List<RolDTO> listaRolDTO = new List<RolDTO>();


            foreach (var rol in listaRol)
            {
                RolDTO rolDTO = rolDAOaDTO(rol);


                listaRolDTO.Add(rolDTO);
            }


            return listaRolDTO;
        }
    }
}
