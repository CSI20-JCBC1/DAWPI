using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DAWPI.Pages.Administrador
{
    [Authorize(Roles = "Admin")]
    public class AccionesModel : PageModel
    {
        private readonly ILogger<AccionesModel> _logger;
        private readonly string _logFilePath;

        public AccionesModel(ILogger<AccionesModel> logger)
        {
            _logger = logger;
            _logFilePath = Path.Combine("C:", "logs", "log.txt");
        }

        public IActionResult OnGet()
        {
            try
            {
                var message = $"Entrando en página principal de administrador: {DateTime.Now}";

                _logger.LogInformation(message); // Registra un mensaje informativo en el registro de la aplicación
                WriteLogToFile(message); // Escribe el mensaje en el archivo de registro
            }
            catch (Exception ex)
            {
                var message = $"Se ha producido una excepción en página principal de administrador: {DateTime.Now}";

                _logger.LogError(ex, message); // Registra la excepción como un error en el registro de la aplicación
                WriteLogToFile(message); // Escribe el mensaje de excepción en el archivo de registro
            }

            return Page(); // Devuelve la página
        }

        public IActionResult OnPostPacientes()
        {
            return RedirectToPage("/Administrador/Pacientes"); // Redirecciona a la página de Pacientes
        }

        public IActionResult OnPostMedicos()
        {
            return RedirectToPage("/Administrador/Medicos"); // Redirecciona a la página de Médicos
        }

        private void WriteLogToFile(string message)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)); // Crea el directorio si no existe

                // Abre el archivo de registro en modo de escritura y añade el mensaje al final
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine(message); // Escribe el mensaje en el archivo de registro
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al escribir en el archivo de registro."); // Registra la excepción como un error en el registro de la aplicación
            }
        }
    }
}
