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
                // Obtener el correo electrónico y el código de verificación almacenados en la sesión
                Email = HttpContext.Session.GetString("Email");
                Code = HttpContext.Session.GetString("Code");

                Console.WriteLine(Code);
                Console.WriteLine(Codigo);

                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Code))
                {
                    ModelState.AddModelError(string.Empty, "Error: No se encontró información de verificación en la sesión.");
                    return Page();
                }

                if (Code == Codigo)
                {
                    // El código de verificación es válido

                    var message = $"Código para guardar contraseña verificado con éxito: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message);
                    WriteLogToFile(message);

                    HttpContext.Session.SetString("Email", Email);
                    return RedirectToPage("/Login/CambiarContraseña");
                }
                else
                {
                    // El código de verificación no es válido
                    ModelState.AddModelError(string.Empty, "Error: El código de verificación no es válido.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al procesar la solicitud
                ModelState.AddModelError(string.Empty, "Ocurrió un error al procesar la solicitud.");
                _logger.LogError(ex, "Error al cambiar la contraseña");
                WriteLogToFile($"Se ha producido una excepción al intentar borrar paciente o médico: {DateTime.Now.ToString()}");
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
