using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using DAWPI.Pages.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Medicos
{
    [Authorize(Roles = "Médico")]
    public class CitasModel : PageModel
    {
        [BindProperty]
        public int detalle { get; set; }
        public string EmailUsuario { get; set; }

        private readonly ILogger<CitasModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public CitasModel(DatabasePiContext db, ILogger<CitasModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public List<CitaDTO> listaCitasDTO { get; set; }
        public List<CatEstadoCitaDTO> listaEstadoDTO { get; set; }
        public UsuarioDTO usuarioDTO { get; set; }

        public void OnGet()
        {
            try
            {
                var message = $"Entrando en página principal de médicos: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                EmailUsuario = User.FindFirst("EmailUsuario")?.Value;

                // Obtener el usuario actual a partir del correo electrónico
                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                usuarioDTO= UsuarioDAOaDTO.usuarioDAOaDTO(usuario);

                // Obtener todas las citas
                List<Cita> listaCitas = _db.Citas.ToList();

                // Filtrar las citas del médico actual
                List<Cita> citasUsuarioSeleccionado = listaCitas.Where(c => c.NombreMedico == usuario.NombreCompleto).ToList();

                // Convertir las citas a DTO
                listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(citasUsuarioSeleccionado);

                // Obtener la lista de estados de cita
                List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();

                // Asignar los nombres de estado de cita correspondientes a cada cita
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

                // Obtener la lista de salas de cita
                List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();

                // Asignar los nombres de sala de cita correspondientes a cada cita
                foreach (CitaDTO cita in listaCitasDTO)
                {
                    foreach (var sala in listaSalaCita)
                    {
                        if (cita.CodSala == sala.CodSala)
                        {
                            cita.CodSala = sala.NombreSala;
                        }
                    }
                }

                // Obtener la lista de plantas de cita
                List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();

                // Asignar los nombres de planta de cita correspondientes a cada cita
                foreach (CitaDTO cita in listaCitasDTO)
                {
                    foreach (var planta in listaPlantaCita)
                    {
                        if (cita.CodPlanta == planta.CodPlanta)
                        {
                            cita.CodPlanta = planta.NombrePlanta;
                        }
                    }
                }

                // Obtener la lista de estados de cita y convertirlos a DTO
                List<CatEstadoCitum> listaEstados = _db.CatEstadoCita.ToList();
                listaEstadoDTO = CatEstadoCitaDAOaDTO.listaCatEstadoCitaDAOaDTO(listaEstados);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Excepción ocurrida en la página principal de médicos: {DateTime.Now.ToString()}");
            }
        }

        public IActionResult OnPostDetalles()
        {
            // Establecer el valor de "detalle" en la sesión y redirigir a la página "DetallesCita"
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./DetallesCita");
        }

        public IActionResult OnPostDetalles2()
        {
            // Establecer el valor de "detalle" en la sesión y redirigir a la página "DetallesCita2"
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./DetallesCita2");
        }

        public IActionResult OnPostDetalles3()
        {
            // Establecer el valor de "detalle" en la sesión y redirigir a la página "DetallesCita3"
            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./DetallesCita3");
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
