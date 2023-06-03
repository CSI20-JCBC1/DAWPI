using DAL.DTO;
using DAL.Models;
using DAWPI.Pages.Medicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Register
{
    public class VerificacionModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Contrasenia { get; set; }

        private readonly ILogger<VerificacionModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public VerificacionModel(DatabasePiContext db, ILogger<VerificacionModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }
        public IActionResult OnPostVerificar()
        {

            try
            {
                var message = $"Entrando en p�gina de verificaci�n de usuario: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                var usuario = new Usuario();
                List<Usuario> listausuarios = _db.Usuarios.ToList();
                bool existe = false;
                foreach (Usuario usu in listausuarios)
                {
                    if (Email == usu.Email)
                    {
                        usuario = _db.Usuarios.FirstOrDefault(e => e.Email == Email);
                        existe = true; break;
                    }
                }


                if (existe)
                {
                    if (usuario.Verificado == true)
                    {
                        ModelState.AddModelError(string.Empty, "Su usuario ya est� verificado."); // Se agrega un mensaje de error al modelo de estado
                        return Page(); // Se devuelve la p�gina de inicio de sesi�n para mostrar el mensaje de error al usuario
                    }

                    

                    if (BCrypt.Net.BCrypt.Verify(Contrasenia, usuario.Contrasenya))
                    {

                        usuario.Verificado = true;
                        _db.SaveChanges();

                        message = $"Usuario verificado: {DateTime.Now.ToString()}";
                        _logger.LogInformation(message);
                        WriteLogToFile(message);

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Contrase�a incorrecta, asegurese de introducir bien su contrase�a."); // Se agrega un mensaje de error al modelo de estado
                        return Page(); // Se devuelve la p�gina de inicio de sesi�n para mostrar el mensaje de error al usuario
                    }

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre de usuario no existe, por favor asegureser de introducir bien su nombre."); // Se agrega un mensaje de error al modelo de estado
                    return Page(); // Se devuelve la p�gina de inicio de sesi�n para mostrar el mensaje de error al usuario
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepci�n en la p�gina de verificaci�n de usuario: {DateTime.Now.ToString()}");
            }
            TempData["MensajeExito"] = "Verificaci�n exitosa. Puedes iniciar sesi�n.";
            return RedirectToPage("../Login/Login");
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

