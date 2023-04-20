using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOaDTO
{
    public class UsuarioDAOaDTO
    {
        public static UsuarioDTO usuarioDAOaDTO(Usuario usuario) {
            
            UsuarioDTO usuarioDTO = new UsuarioDTO();

            usuarioDTO.Id = usuario.Id;
            usuarioDTO.MdUuid = usuario.MdUuid;
            usuarioDTO.MdDate = usuario.MdDate;
            usuarioDTO.NombreCompleto = usuario.NombreCompleto;
            usuarioDTO.Movil= usuario.Movil;
            usuarioDTO.Email = usuario.Email;
            usuarioDTO.Contrasenya = usuario.Contrasenya;
            usuarioDTO.Rol=usuario.Rol;

            return usuarioDTO;
        
        }

        public static List<UsuarioDTO> listaUsuarioDAOaDTO(List<Usuario> listaUsuario)
        {
            // Creamos una lista vacía de EmpleadoDTO
            List<UsuarioDTO> listaUsuarioDTO = new List<UsuarioDTO>();

            // Recorremos la lista de objetos DlkCatAccEmpleado y los convertimos uno a uno a EmpleadoDTO
            foreach (var usuario in listaUsuario)
            {
                UsuarioDTO usuarioDTO = usuarioDAOaDTO(usuario);

                // Añadimos el objeto EmpleadoDTO a la lista
                listaUsuarioDTO.Add(usuarioDTO);
            }

            // Devolvemos la lista de objetos EmpleadoDTO
            return listaUsuarioDTO;
        }
    }
}
