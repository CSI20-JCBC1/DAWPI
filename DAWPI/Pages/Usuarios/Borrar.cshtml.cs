using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using DAWPI.Pages.Medicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Usuarios
{
    public class BorrarModel : PageModel
    {
        private readonly ILogger<BorrarModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public BorrarModel(DatabasePiContext db, ILogger<BorrarModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public CitaDTO citaDTO { get; set; }
        public int? detalle { get; set; }

        public void OnGet()
        {
            try
            {
                var message = $"Entrando en página para ver detalles de la cita del médico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                   Cita? cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    if (cita == null)
                    {
                        Response.Redirect("/Usuarios/Citas");
                    }
                    else
                    {
                        citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepción en página para borrar cita usuario: {DateTime.Now.ToString()}");
            }
        }


        public IActionResult OnPost(string confirmacion)
        {
            try
            {
                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                detalle = HttpContext.Session.GetInt32("detalle");
                Console.WriteLine(detalle);
                if (detalle.HasValue)
                {
                    Cita? cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    if (cita == null)
                    {
                        Response.Redirect("/Usuarios/Citas");
                    }
                    else
                    {
                        citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    }
                }

                if (!string.IsNullOrEmpty(confirmacion) && confirmacion.Trim() == citaDTO.Asunto)
                {
                    Cita? cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    _db.Remove(cita);
                    _db.SaveChanges();

                    var message = $"Cita borrada con éxito: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message);
                    WriteLogToFile(message);


                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre completo no coincide. Inténtalo de nuevo.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Entrando en página para ver detalles de la cita del médico: {DateTime.Now.ToString()}");
            }

            return RedirectToPage("/Usuarios/Citas");
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
