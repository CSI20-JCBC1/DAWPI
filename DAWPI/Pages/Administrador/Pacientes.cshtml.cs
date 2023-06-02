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
                var message = $"Entrando en página para la gestión de pacientes: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                List<Usuario> usuarios = _db.Usuarios.Where(u => u.Rol == 2).ToList();
                usuariosDTO = UsuarioDAOaDTO.listaUsuarioDAOaDTO(usuarios);

            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Excepción en la página para gestion de pacientes: {DateTime.Now.ToString()}");

            }

        }
        public IActionResult OnPostBorrar()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./Borrar");
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

