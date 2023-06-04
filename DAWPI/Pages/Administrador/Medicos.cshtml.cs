using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Administrador
{
    [Authorize(Roles = "Admin")]
    public class MedicosModel : PageModel
    {
        // Propiedad para el detalle del m�dico
        [BindProperty]
        public int detalle { get; set; }

        private readonly ILogger<MedicosModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public MedicosModel(DatabasePiContext db, ILogger<MedicosModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        // Lista de m�dicos en formato DTO
        public List<UsuarioDTO> usuariosDTO { get; set; }

        // M�todo ejecutado al cargar la p�gina
        public void OnGet()
        {
            try
            {
                var message = $"Entrando en p�gina para la gesti�n de m�dicos: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Obtener todos los usuarios con rol de m�dico
                List<Usuario> usuarios = _db.Usuarios.Where(u => u.Rol == 1).ToList();

                // Convertir los usuarios al formato DTO
                usuariosDTO = UsuarioDAOaDTO.listaUsuarioDAOaDTO(usuarios);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Se ha producido una excepci�n en la gesti�n de m�dicos: {DateTime.Now.ToString()}");
            }
        }

        // M�todo ejecutado al enviar el formulario de crear m�dico
        public IActionResult OnPostCrear()
        {
            return RedirectToPage("./CrearMedico/");
        }

        // M�todo ejecutado al enviar el formulario de gestionar citas
        public IActionResult OnPostGestionCitas()
        {
            // Guardar el detalle en la sesi�n
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./GestionCitas");
        }

        // M�todo ejecutado al enviar el formulario de borrar m�dico
        public IActionResult OnPostBorrar()
        {
            // Guardar el detalle en la sesi�n
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./Borrar");
        }

        // M�todo para escribir un mensaje de registro en un archivo de registro
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
