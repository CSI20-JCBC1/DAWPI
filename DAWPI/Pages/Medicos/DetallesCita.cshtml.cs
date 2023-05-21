using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public string Fecha { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El campo Hora es obligatorio.")]
        public string Hora { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public string Enfermedad { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El campo Hora es obligatorio.")]
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

        public IActionResult OnPostAsignar()
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
                    // Parsea la fecha y hora ingresadas
                    DateOnly fechaCita = DateOnly.ParseExact(Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    TimeOnly horaCita = TimeOnly.ParseExact(Hora, "HH:mm", CultureInfo.InvariantCulture);

                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                    // Obtener las citas del médico para la misma fecha
                    List<Cita> citasMedico = _db.Citas
                        .Where(c => c.NombreMedico == usuario.NombreCompleto && c.Fecha == fechaCita)
                        .ToList();

                    bool horaInvalida = false;

                    // Verifica si la hora seleccionada está disponible
                    foreach (Cita citaMedico in citasMedico)
                    {
                        TimeOnly horaCitaMedico = (TimeOnly)citaMedico.Hora;
                        TimeOnly horaAbajo = horaCitaMedico.AddMinutes(-30);
                        TimeOnly horaArriba = horaCitaMedico.AddMinutes(30);

                        if (horaCita >= horaAbajo && horaCita <= horaArriba)
                        {
                            horaInvalida = true;
                            break;
                        }
                    }

                    if (!horaInvalida)
                    {
                        // Actualiza la cita con la nueva fecha, hora y estado
                        cita.Fecha = fechaCita;
                        cita.Hora = horaCita;
                        cita.EstadoCita = "F";
                        _db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "La hora de la cita seleccionada no está disponible. Por favor, elige otra hora."); // Agrega un mensaje de error al modelo de estado
                        return Page();
                    }
                }
            }

            // Redirecciona a la página de citas
            return RedirectToPage("/Medicos/Citas");
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
                    _db.SaveChanges();
                }
            }

            // Redirecciona a la página de citas
            return RedirectToPage("/Medicos/Citas");
        }
    }

}
