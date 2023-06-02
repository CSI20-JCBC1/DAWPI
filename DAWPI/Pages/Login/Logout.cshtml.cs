using DAL.Models;
using DAWPI.Pages.Administrador;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Login
{
    public class LogoutModel : PageModel
    {

        private readonly ILogger<LogoutModel> _logger;
        private readonly string _logFilePath;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                //Fijamos el logout en nuestro esquema de autenticaci�n y una vez este se efect�a redirigimos a la p�gina de login
                await HttpContext.SignOutAsync("AuthScheme");

                var message = $"Sesi�n cerrada: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogInformation(ex.ToString());
                WriteLogToFile("Excepci�n producida en el inicio de sesi�n");
            }


            return RedirectToPage("./Login");
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
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

