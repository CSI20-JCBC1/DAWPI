using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Globalization;
using System.Linq;

namespace DAWPI.Pages.Medicos
{
    [Authorize(Roles = "Médico")]
    public class DetallesCitaModel : PageModel
    {
        private readonly DatabasePiContext _db;
        public DetallesCitaModel(DatabasePiContext db)
        {
            _db = db;
        }

        [BindProperty]
        public CitaDTO Cita { get; set; }

        [BindProperty]
        public string fecha { get; set; }

        [BindProperty]
        public string hora { get; set; }

        public void OnGet()
        {
            int? detalle = HttpContext.Session.GetInt32("detalle");
            if (detalle.HasValue)
            {
                Cita cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                Cita = citaDTO;
            }
        }

        public IActionResult OnPostAsignar()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string emailUsuario = User.FindFirst("EmailUsuario")?.Value;
            int? detalle = HttpContext.Session.GetInt32("detalle");
            if (detalle.HasValue)
            {
                Cita cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                if (cita != null)
                {
                    DateOnly fechaCita = DateOnly.ParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    TimeOnly horaCita = TimeOnly.ParseExact(hora, "HH:mm", CultureInfo.InvariantCulture);

                    // Obtener las citas del médico para la misma fecha
                    List<Cita> citasMedico = _db.Citas
                        .Where(c => c.NombreMedico == emailUsuario && c.Fecha == fechaCita)
                        .ToList();


                    foreach (Cita citaMedico in citasMedico)
                    {
                        TimeOnly horaAbajo = citaMedico.Hora.Value.AddMinutes(30);
                        TimeOnly horaArriba = citaMedico.Hora.Value.AddMinutes(30);

                        if (horaCita.IsBetween(horaAbajo, horaArriba))
                        {
                            ModelState.AddModelError("", "La hora seleccionada coincide con otra cita del médico.");
                            return Page();
                        }
                    }

                    cita.Fecha = fechaCita;
                    cita.Hora = horaCita;
                    cita.EstadoCita = "A";
                    _db.SaveChanges();
                }
            }

            return RedirectToPage("/Medicos/Citas");
        }

    }
}