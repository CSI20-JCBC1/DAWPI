using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using DAL.Models;
using DAL.DAOaDTO;
using DAL.DTO;

namespace DAWPI.Pages.Usuarios
{
    [Authorize(Roles = "Paciente")]
    public class CitasModel : PageModel
    {
        public string EmailUsuario { get; set; }

        private readonly DatabasePiContext _db;
        public CitasModel(DatabasePiContext db)
        {
            _db = db;
        }

        public List<CitaDTO> listaCitasDTO { get; set; }

        public void OnGet()
        {
            EmailUsuario = "jcbc20012004@gmail.com";

            Console.WriteLine("Correo "+EmailUsuario);

            try {
                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                List<Cita> listaCitas = _db.Citas.ToList();

                List<Cita> citasUsuarioSeleccionado = listaCitas.Where(c => c.NombrePaciente == usuario.NombreCompleto).ToList();

                listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(citasUsuarioSeleccionado);
                
            }
            catch(Exception e) { 

                Console.WriteLine(e);

            }

        }
    }
}
