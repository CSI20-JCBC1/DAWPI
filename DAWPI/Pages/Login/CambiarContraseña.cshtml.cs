using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Login
{
    public class CambiarContraseñaModel : PageModel
    {
        private readonly ILogger<CambiarContraseñaModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public CambiarContraseñaModel(DatabasePiContext db, ILogger<CambiarContraseñaModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        [BindProperty]
        public string contrasenia { get; set; }
        [BindProperty]
        public string contrasenia2 { get; set; }

        public string Email { get; private set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                Email = HttpContext.Session.GetString("Email");
                var usuario = _db.Usuarios.FirstOrDefault(e => e.Email == Email);
                if (usuario != null)
                {
                    if (contrasenia.Equals(contrasenia2))
                    {
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(contrasenia);
                        // Asignar la contraseña encriptada al objeto Usuario
                        usuario.Contrasenya = hashedPassword;
                        _db.SaveChanges();
                        return RedirectToPage("/Login/Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error, las contraseñas no coinciden"); // Se agrega un mensaje de error al modelo de estado
                        return Page();
                    }
                }
                else
                {
                    return Page();
                }

            }
            catch (Exception ex)
            {
                return Page();
            }


        }
    }
}
