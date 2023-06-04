using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Login
{
    public class VerificacionCodigoModel : PageModel
    {
        private readonly ILogger<VerificacionCodigoModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public VerificacionCodigoModel(DatabasePiContext db, ILogger<VerificacionCodigoModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public string Email { get; private set; }
        public string Code { get; private set; }

        [BindProperty]
        public string Codigo { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            try
            {
                // Obtener el correo electr�nico y el c�digo de verificaci�n almacenados en la sesi�n
                Email = HttpContext.Session.GetString("Email");
                Code = HttpContext.Session.GetString("Code");

                Console.WriteLine(Code);
                Console.WriteLine(Codigo);

                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Code))
                {
                    ModelState.AddModelError(string.Empty, "Error: No se encontr� informaci�n de verificaci�n en la sesi�n.");
                    return Page();
                }

                if (Code == Codigo)
                {
                    // El c�digo de verificaci�n es v�lido

                    var message = $"C�digo para guardar contrase�a verificado con �xito: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message);
                    WriteLogToFile(message);

                    HttpContext.Session.SetString("Email", Email);
                    return RedirectToPage("/Login/CambiarContrase�a");
                }
                else
                {
                    // El c�digo de verificaci�n no es v�lido
                    ModelState.AddModelError(string.Empty, "Error: El c�digo de verificaci�n no es v�lido.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Ocurri� un error al procesar la solicitud
                ModelState.AddModelError(string.Empty, "Ocurri� un error al procesar la solicitud.");
                _logger.LogError(ex, "Error al cambiar la contrase�a");
                WriteLogToFile($"Se ha producido una excepci�n al intentar borrar paciente o m�dico: {DateTime.Now.ToString()}");
                return Page();
            }
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
