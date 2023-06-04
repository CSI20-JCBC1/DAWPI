using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using DAWPI.Pages.Login;
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
        private readonly ILogger<DetallesCitaModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public DetallesCitaModel(DatabasePiContext db, ILogger<DetallesCitaModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
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
                var message = $"Entrando en página para ver detalles de la cita del médico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Desactiva el caché para la página
                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";

                Detalle = HttpContext.Session.GetInt32("detalle");
                if (Detalle.HasValue)
                {
                    // Obtiene la cita correspondiente al detalle
                    Cita cita = _db.Citas.FirstOrDefault(c => c.Id == Detalle);
                    CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    Cita = citaDTO;

                    // Obtiene la lista de estados de cita
                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();

                    // Asigna el nombre del estado de cita correspondiente a la cita actual
                    foreach (var estadoCita in listaEstadoCita)
                    {
                        if (Cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            Cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }

                    // Obtiene la lista de salas de cita
                    List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();

                    // Asigna el nombre de la sala de cita correspondiente a la cita actual
                    foreach (var sala in listaSalaCita)
                    {
                        if (Cita.CodSala == sala.CodSala)
                        {
                            Cita.CodSala = sala.NombreSala;
                        }
                    }

                    // Obtiene la lista de plantas de cita
                    List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();

                    // Asigna el nombre de la planta de cita correspondiente a la cita actual
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
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepción en la página de detalles de la cita del médico: {DateTime.Now.ToString()}");
            }
        }

        public IActionResult OnPostAsignar()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                Detalle = HttpContext.Session.GetInt32("detalle");

                string emailUsuario = User.FindFirst("EmailUsuario")?.Value;
                if (Detalle.HasValue)
                {
                    // Obtiene la cita correspondiente al detalle
                    Cita cita = _db.Citas.FirstOrDefault(c => c.Id == Detalle);
                    CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    Cita = citaDTO;
                    if (cita != null)
                    {
                        // Parsea la fecha y hora ingresadas
                        DateOnly fechaCita = DateOnly.ParseExact(Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        TimeOnly horaCita = TimeOnly.ParseExact(Hora, "HH:mm", CultureInfo.InvariantCulture);

                        Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                        // Obtiene las citas del médico para la misma fecha
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
                            cita.EstadoCita = "A";
                            _db.SaveChanges();

                            var message = $"Fecha y hora asignadas a la cita con éxito: {DateTime.Now.ToString()}";
                            _logger.LogInformation(message);
                            WriteLogToFile(message);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "La fecha introducida o la hora no están disponibles, intente insertar una fecha válida o que el horario de citas en el mismo día no coincidan"); // Agrega un mensaje de error al modelo de estado
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepción al intentar asignar la cita: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Error al asignar la cita, intentelo más tarde.");
                return Page();
            }

            // Redirecciona a la página de citas
            return RedirectToPage("/Medicos/Citas");
        }

        private void WriteLogToFile(string message)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }
        }
    }
}
