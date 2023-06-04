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
                // Obtiene el email almacenado en la sesión
                Email = HttpContext.Session.GetString("Email");

                // Busca el usuario correspondiente al email en la base de datos
                var usuario = _db.Usuarios.FirstOrDefault(e => e.Email == Email);

                if (usuario != null)
                {
                    if (contrasenia.Equals(contrasenia2))
                    {
                        // Encripta la nueva contraseña utilizando BCrypt
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(contrasenia);

                        // Asigna la contraseña encriptada al objeto Usuario
                        usuario.Contrasenya = hashedPassword;
                        _db.SaveChanges();

                        // Registro exitoso en los logs
                        var message = $"Contraseña cambiada con éxito: {DateTime.Now.ToString()}";
                        _logger.LogInformation(message);
                        WriteLogToFile(message);

                        return RedirectToPage("/Login/Login");
                    }
                    else
                    {
                        // Las contraseñas no coinciden, se agrega un mensaje de error al modelo de estado
                        ModelState.AddModelError(string.Empty, "Error, las contraseñas no coinciden");
                        return Page();
                    }
                }
                else
                {
                    // No se encontró el usuario correspondiente al email, se agrega un mensaje de error al modelo de estado
                    ModelState.AddModelError(string.Empty, "Se produjo un error al cambiar la contraseña. Por favor, intenta nuevamente más tarde.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Registra la excepción en los logs y muestra un mensaje de error
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepción producida a la hora de cambiar la contraseña: {DateTime.Now.ToString()}");
                return Page();
            }
        }

        private void WriteLogToFile(string message)
        {
            try
            {
                // Crea el directorio para el archivo de log si no existe
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    // Escribe el mensaje en el archivo de log
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                // Registra la excepción en los logs
                _logger.LogInformation(ex.ToString());
            }
        }
    }
}
