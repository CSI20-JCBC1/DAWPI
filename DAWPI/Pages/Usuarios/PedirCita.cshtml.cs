using DAL.DTO;
using DAL.DTOaDAO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Usuarios
{
    [Authorize(Roles = "Paciente")]
    public class PedirCitaModel : PageModel
    {
        private readonly DatabasePiContext _db;
        [BindProperty]
        public string Asunto { get; set; }

        [BindProperty]
        public string Sintomas { get; set; }

        public string EmailUsuario { get; set; }

        public PedirCitaModel(DatabasePiContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try {

                EmailUsuario = User.FindFirst("EmailUsuario")?.Value;

                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                    CitaDTO citaDTO = new CitaDTO(Asunto, usuario.NombreCompleto, Sintomas," ");

                    Cita cita = CitaDTOaDAO.citaDTOaDAO(citaDTO);

                if (cita == null)
                {
                    ModelState.AddModelError(string.Empty, "Error al solicitar la cita, ahora mismo es imposible crear la cita debido a algunos problemas, inténtelo más tarde."); // Se agrega un mensaje de error al modelo de estado
                    return Page(); // Se devuelve la página de inicio de sesión para mostrar el mensaje de error al usuario
                }
                else
                {
                    _db.Add(cita);
                    _db.SaveChanges();
                }
                
            }catch (Exception e) 
            {
                Console.WriteLine(e);
            }

            return RedirectToPage("/Usuarios/Citas");
        }
    }
}
