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
                //Fijamos el logout en nuestro esquema de autenticación y una vez este se efectúa redirigimos a la página de login
                await HttpContext.SignOutAsync("AuthScheme");

                var message = $"Sesión cerrada: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogInformation(ex.ToString());
                WriteLogToFile("Excepción producida en el inicio de sesión");
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

