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

                    HttpContext.Session.SetString("Email", Email);
                    return RedirectToPage("/Login/CambiarContrase�a");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error: El c�digo de verificaci�n no es v�lido.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurri� un error al procesar la solicitud.");
                _logger.LogError(ex, "Error al cambiar la contrase�a");
                return Page();
            }
        }
    }
}
