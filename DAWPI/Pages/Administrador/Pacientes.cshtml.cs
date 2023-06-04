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
    public class PacientesModel : PageModel
    {
        [BindProperty]
        public int detalle { get; set; }

        private readonly ILogger<PacientesModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public PacientesModel(DatabasePiContext db, ILogger<PacientesModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public List<UsuarioDTO> usuariosDTO { get; set; }

        public void OnGet()
        {
            try
            {
                // Registra la entrada en la p�gina en los logs
                var message = $"Entrando en p�gina para la gesti�n de pacientes: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Obtiene la lista de usuarios con rol 2 (pacientes)
                List<Usuario> usuarios = _db.Usuarios.Where(u => u.Rol == 2).ToList();

                // Convierte la lista de usuarios a una lista de DTOs
                usuariosDTO = UsuarioDAOaDTO.listaUsuarioDAOaDTO(usuarios);
            }
            catch (Exception e)
            {
                // Registra la excepci�n en los logs
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Excepci�n en la p�gina para gestion de pacientes: {DateTime.Now.ToString()}");
            }
        }

        public IActionResult OnPostBorrar()
        {
            // Almacena el valor de "detalle" en la sesi�n y redirige a la p�gina "Borrar"
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./Borrar");
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
                // Registra la excepci�n en los logs
                _logger.LogInformation(ex.ToString());
            }
        }
    }
}
