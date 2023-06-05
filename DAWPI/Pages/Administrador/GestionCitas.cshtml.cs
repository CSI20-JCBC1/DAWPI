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
    public class GestionCitasModel : PageModel
    {
        private readonly ILogger<GestionCitasModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public GestionCitasModel(DatabasePiContext db, ILogger<GestionCitasModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        // Propiedad para el detalle de la cita
        [BindProperty]
        public int detalle { get; set; }

        // Propiedad para el ID del usuario
        public long usuId { get; set; }

        // Lista de citas en formato DTO
        public List<CitaDTO> listaCitasDTO { get; set; }
        //Para mostrar el nombre del m�dico en la lista
        public UsuarioDTO usuarioDTO { get; set; }

        // M�todo ejecutado al cargar la p�gina
        public void OnGet(/*int detalle*/)
        {
            try
            {
                var message = $"Entrando en p�gina para gestionar citas del m�dico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Obtener el detalle de la sesi�n
                int? detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    // Obtener el usuario correspondiente al detalle
                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);
                    usuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);

                    usuId = usuario.Id;

                    // Obtener todas las citas de la base de datos
                    List<Cita> listaCitas = _db.Citas.ToList();

                    // Filtrar las citas del usuario seleccionado
                    List<Cita> citasUsuarioSeleccionado = listaCitas.Where(c => c.NombreMedico == usuario.NombreCompleto).ToList();

                    // Convertir las citas al formato DTO
                    listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(citasUsuarioSeleccionado);

                    // Obtener la lista de estados de cita
                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();

                    // Reemplazar los c�digos de estado de cita por sus descripciones correspondientes
                    foreach (CitaDTO cita in listaCitasDTO)
                    {
                        foreach (var estadoCita in listaEstadoCita)
                        {
                            if (cita.EstadoCita == estadoCita.EstadoCita)
                            {
                                cita.EstadoCita = estadoCita.DescEstadoCita;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Se ha producido una excepci�n en la p�gina para gestionar citas del m�dico: {DateTime.Now.ToString()}");
            }
        }

        // M�todo ejecutado al enviar el formulario de nueva cita
        public IActionResult OnPostNuevaCita()
        {
            // Guardar el detalle en la sesi�n
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./NuevaCita");
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
