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
        [BindProperty]
        public int detalle { get; set; }
        public string EmailUsuario { get; set; }

        private readonly DatabasePiContext _db;
        public CitasModel(DatabasePiContext db)
        {
            _db = db;
        }

        public List<CitaDTO> listaCitasDTO { get; set; }

        public void OnGet()
        {
            EmailUsuario = User.FindFirst("EmailUsuario")?.Value;

            try {

                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                List<Cita> listaCitas = _db.Citas.ToList();

                List<Cita> citasUsuarioSeleccionado = listaCitas.Where(c => c.NombrePaciente == usuario.NombreCompleto).ToList();

                listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(citasUsuarioSeleccionado);

                List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();

                foreach (CitaDTO cita in listaCitasDTO)
                {
                    foreach (var estadoCita in  listaEstadoCita)
                    {
                        if (cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }
                }
                
            }
            catch(Exception e) { 

                Console.WriteLine(e);

            }

        }

        
        public IActionResult OnPostCrear()
        {
            return RedirectToPage("/Usuarios/PedirCita");
        }

        public IActionResult OnPostDetalles()
        {

            return RedirectToPage("./DetallesCita/", new { detalle = detalle });

        }

    }
}