using DAL.DTO;
using DAL.DTOaDAO;
using DAL.Models;
using DAWPI.Pages.Medicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Usuarios
{
    [Authorize(Roles = "Paciente")]
    public class PedirCitaModel : PageModel
    {

        [BindProperty]
        public string Asunto { get; set; }

        [BindProperty]
        public string Sintomas { get; set; }

        public string EmailUsuario { get; set; }

        private readonly ILogger<PedirCitaModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public PedirCitaModel(DatabasePiContext db, ILogger<PedirCitaModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public void OnGet()
        {
            try
            {
                var message = $"Entrando en página para pedir cita: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);
            }
            catch (Exception e)
            {

                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Excepcion en la página pedir cita: {DateTime.Now.ToString()}");
            }

        }

        public IActionResult OnPost()
        {
            try
            {


                EmailUsuario = User.FindFirst("EmailUsuario")?.Value;

                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                CitaDTO citaDTO = new CitaDTO(Asunto, usuario.NombreCompleto, Sintomas);

                Cita cita = CitaDTOaDAO.citaDTOaDAO(citaDTO);

                if (cita == null)
                {
                    ModelState.AddModelError(string.Empty, "Error al solicitar la cita, ahora mismo es imposible crear la cita debido a algunos problemas, inténtelo más tarde."); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else
                {
                    _db.Add(cita);
                    _db.SaveChanges();

                    var message = $"Cita pedida con éxito: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message);
                    WriteLogToFile(message);
                }

            }
            catch (Exception e)
            {

                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Excepcion provocada al pedir cita: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Error al solicitar la cita, ahora mismo es imposible crear la cita debido a algunos problemas, inténtelo más tarde."); // Se agrega un mensaje de error al modelo de estado
                return Page();
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
