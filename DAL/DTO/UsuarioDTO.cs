using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class UsuarioDTO
    {
        //Propiedades
        private string mdUuid;

        private DateTime mdDate;

        private long id;

        private string nombreCompleto;

        private string movil;

        private string email;

        private string contrasenya;

        private long rol;



        //Constructores

        public UsuarioDTO(long id, string nombreCompleto, string movil, string email, string contrasenya)
        {
            this.MdUuid = Guid.NewGuid().ToString();
            this.MdDate = DateTime.Now;
            this.Id = id;
            this.NombreCompleto = nombreCompleto;
            this.Movil = movil;
            this.Email = email;
            this.Contrasenya = contrasenya;
            this.Rol = 2;


        }

        public UsuarioDTO(string nombreCompleto, string movil, string email, string contrasenya)
        {
            this.MdUuid = Guid.NewGuid().ToString();
            this.MdDate = DateTime.Now;
            this.NombreCompleto = nombreCompleto;
            this.Movil = movil;
            this.Email = email;
            this.Contrasenya = contrasenya;
            this.Rol = 2;
        }

        public UsuarioDTO(string nombreCompleto, string movil, string email, string contrasenya,long rol)
        {
            this.MdUuid = Guid.NewGuid().ToString();
            this.MdDate = DateTime.Now;
            this.NombreCompleto = nombreCompleto;
            this.Movil = movil;
            this.Email = email;
            this.Contrasenya = contrasenya;
            this.Rol = rol;


        }

        public UsuarioDTO()
        {

        }

        //Getters & Setters
        public string MdUuid { get => mdUuid; set => mdUuid = value; }
        public DateTime MdDate { get => mdDate; set => mdDate = value; }
        public long Id { get => id; set => id = value; }
        public string NombreCompleto { get => nombreCompleto; set => nombreCompleto = value; }
        public string Movil { get => movil; set => movil = value; }
        public string Email { get => email; set => email = value; }
        public string Contrasenya { get => contrasenya; set => contrasenya = value; }
        public long Rol { get => rol; set => rol = value; }
    }
}
