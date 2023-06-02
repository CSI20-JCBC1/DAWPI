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
            _logFilePath = @"C:\logs\log.txt";
        }

        public void OnGet()
        {
            try
            {
                var message = $"Entrando en página principal de administrador: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepción en página principal de administrador: {DateTime.Now.ToString()}");

            }
        }

        public IActionResult OnPostPacientes()
        {
            return RedirectToPage("/Administrador/Pacientes");
        }

        public IActionResult OnPostMedicos()
        {
            return RedirectToPage("/Administrador/Medicos");
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
