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
        // Propiedad para el detalle del médico
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

        // Lista de médicos en formato DTO
        public List<UsuarioDTO> usuariosDTO { get; set; }

        // Método ejecutado al cargar la página
        public void OnGet()
        {
            try
            {
                var message = $"Entrando en página para la gestión de médicos: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Obtener todos los usuarios con rol de médico
                List<Usuario> usuarios = _db.Usuarios.Where(u => u.Rol == 1).ToList();

                // Convertir los usuarios al formato DTO
                usuariosDTO = UsuarioDAOaDTO.listaUsuarioDAOaDTO(usuarios);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Se ha producido una excepción en la gestión de médicos: {DateTime.Now.ToString()}");
            }
        }

        // Método ejecutado al enviar el formulario de crear médico
        public IActionResult OnPostCrear()
        {
            return RedirectToPage("./CrearMedico/");
        }

        // Método ejecutado al enviar el formulario de gestionar citas
        public IActionResult OnPostGestionCitas()
        {
            // Guardar el detalle en la sesión
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./GestionCitas");
        }

        // Método ejecutado al enviar el formulario de borrar médico
        public IActionResult OnPostBorrar()
        {
            // Guardar el detalle en la sesión
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./Borrar");
        }

        // Método para escribir un mensaje de registro en un archivo de registro
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
