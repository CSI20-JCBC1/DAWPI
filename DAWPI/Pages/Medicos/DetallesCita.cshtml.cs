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
        public string Fecha { get; set; }

        [BindProperty]
        public string Hora { get; set; }


        public int? Detalle { get; set; }

        public void OnGet()
        {
            try
            {
                // Desactiva el caché para la página
                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";

                Detalle = HttpContext.Session.GetInt32("detalle");
                if (Detalle.HasValue)
                {
                    Cita cita = _db.Citas.FirstOrDefault(c => c.Id == Detalle);
                    CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    Cita = citaDTO;

                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();
                    foreach (var estadoCita in listaEstadoCita)
                    {
                        if (Cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            Cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }

                    List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();
                    foreach (var sala in listaSalaCita)
                    {
                        if (Cita.CodSala == sala.CodSala)
                        {
                            Cita.CodSala = sala.NombreSala;
                        }
                    }

                    List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();
                    foreach (var planta in listaPlantaCita)
                    {
                        if (Cita.CodPlanta == planta.CodPlanta)
                        {
                            Cita.CodPlanta = planta.NombrePlanta;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IActionResult OnPostAsignar()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

                    DateTime fechaActual = DateTime.Today;
                    DateOnly fechaHoy = new DateOnly(fechaActual.Year, fechaActual.Month, fechaActual.Day);

                    if (!horaInvalida && !(fechaCita <= fechaHoy))
                    {
                        // Actualiza la cita con la nueva fecha, hora y estado
                        cita.Fecha = fechaCita;
                        cita.Hora = horaCita;
                        cita.EstadoCita = "F";
                        _db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "La fecha introducida o la hora no están disponibles, intente insertar una fecha válida o que el horario de citas en el mismo día no coincidan"); // Agrega un mensaje de error al modelo de estado
                        return Page();
                    }
                }
            }

            // Redirecciona a la página de citas
            return RedirectToPage("/Medicos/Citas");
        }


    }

}
