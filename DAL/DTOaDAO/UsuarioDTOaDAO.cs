using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOaDAO
{
    public class UsuarioDTOaDAO
    {
        public static Usuario usuarioDTOaDAO (UsuarioDTO usuarioDTO)
        {
            Usuario usuario = new Usuario ();

            usuario.Id = usuarioDTO.Id;
            usuario.MdDate = usuarioDTO.MdDate;
            usuario.MdUuid = usuarioDTO.MdUuid;
            usuario.NombreCompleto=usuarioDTO.NombreCompleto;
            usuario.Movil=usuarioDTO.Movil;
            usuario.Email=usuarioDTO.Email;
            usuario.Contrasenya=usuarioDTO.Contrasenya;
            usuario.Rol=usuarioDTO.Rol;

            return usuario;
        }
    }
}
