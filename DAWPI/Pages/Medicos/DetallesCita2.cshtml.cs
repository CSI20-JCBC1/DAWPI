using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Globalization;

namespace DAWPI.Pages.Medicos
{
    [Authorize(Roles = "Médico")]
    public class DetallesCita2Model : PageModel
    {
        private readonly DatabasePiContext _db;

        public DetallesCita2Model(DatabasePiContext db)
        {
            _db = db;
        }

        [BindProperty]
        public CitaDTO Cita { get; set; }

        [BindProperty]
        public string Enfermedad { get; set; }

        [BindProperty]
        public string Solucion { get; set; }


        public int? Detalle { get; set; }

        public void OnGet()
        {
            // Desactiva el caché para la página
            HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";

            Detalle = HttpContext.Session.GetInt32("detalle");
            if (Detalle.HasValue)
            {
                Cita cita = _db.Citas.FirstOrDefault(c => c.Id == Detalle);
                CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                Cita = citaDTO;
            }
        }

        public IActionResult OnPostDiagnosticar()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Desactiva el caché para la página
            HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Detalle = HttpContext.Session.GetInt32("detalle");

            string emailUsuario = User.FindFirst("EmailUsuario")?.Value;
            if (Detalle.HasValue)
            {
                Cita cita = _db.Citas.FirstOrDefault(c => c.Id == Detalle);
                CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                Cita = citaDTO;
                if (cita != null)
                {
                    cita.Enfermedad = Enfermedad;
                    cita.Solucion = Solucion;
                    cita.EstadoCita = "A";
                    _db.SaveChanges();
                }
            }

            // Redirecciona a la página de citas
            return RedirectToPage("/Medicos/Citas");
        }

    }
}

