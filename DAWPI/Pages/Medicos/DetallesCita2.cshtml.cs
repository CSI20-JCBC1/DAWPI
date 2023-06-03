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
    [Authorize(Roles = "M�dico")]
    public class DetallesCita2Model : PageModel
    {
        private readonly ILogger<DetallesCita2Model> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public DetallesCita2Model(DatabasePiContext db, ILogger<DetallesCita2Model> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
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
            try
            {
                // Desactiva el cach� para la p�gina
                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";

                Detalle = HttpContext.Session.GetInt32("detalle");
                if (Detalle.HasValue)
                {
                    Cita cita = _db.Citas.FirstOrDefault(c => c.Id == Detalle);
                    CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    Cita = citaDTO;
                }

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepci�n en la p�gina de detalles de la cita del m�dico: {DateTime.Now.ToString()}");
            }

        }

        public IActionResult OnPostDiagnosticar()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Desactiva el cach� para la p�gina
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

                        var message = $"Diagn�stico realizado con �xito: {DateTime.Now.ToString()}";
                        _logger.LogInformation(message);
                        WriteLogToFile(message);
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepci�n ocurrida al diagnosticar la enfermedad del paciente: {DateTime.Now.ToString()}");
            }


            // Redirecciona a la p�gina de citas
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

