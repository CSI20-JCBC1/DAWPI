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
        [BindProperty]
        public int detalle { get; set;}

        private readonly ILogger<MedicosModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public MedicosModel(DatabasePiContext db, ILogger<MedicosModel> logger)
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
                var message = $"Entrando en página para la gestión de médicos: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                List<Usuario> usuarios = _db.Usuarios.Where(u => u.Rol == 1).ToList();
                usuariosDTO=UsuarioDAOaDTO.listaUsuarioDAOaDTO(usuarios);
            }
            catch (Exception e)
            {

                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Se ha producido una excepción para la gestión de médicos: {DateTime.Now.ToString()}");

            }

        }

        public IActionResult OnPostCrear()
        {
            return RedirectToPage("./CrearMedico/");
        }

        public IActionResult OnPostGestionCitas()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./GestionCitas");

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
                _logger.LogInformation(ex.ToString());

            }
        }
    }
}
