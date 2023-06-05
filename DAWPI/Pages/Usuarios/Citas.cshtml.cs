using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using DAL.Models;
using DAL.DAOaDTO;
using DAL.DTO;
using DAWPI.Pages.Medicos;

namespace DAWPI.Pages.Usuarios
{
    [Authorize(Roles = "Paciente")]
    public class CitasModel : PageModel
    {
        [BindProperty]
        public int detalle { get; set; }
        public string EmailUsuario { get; set; }

        public UsuarioDTO UsuarioDTO { get; set; }

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

        public void OnGet()
        {
            EmailUsuario = User.FindFirst("EmailUsuario")?.Value;

            try {

                var message = $"Entrando en página para ver citas del paciente: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                UsuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);

                List<Cita> listaCitas = _db.Citas.ToList();

                List<Cita> citasUsuarioSeleccionado = listaCitas.Where(c => c.NombrePaciente == usuario.NombreCompleto).ToList();

                listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(citasUsuarioSeleccionado);

                List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();

                foreach (CitaDTO cita in listaCitasDTO)
                {
                    foreach (var estadoCita in  listaEstadoCita)
                    {
                        if (cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }
                }

                List<CatEstadoCitum> listaEstados = _db.CatEstadoCita.ToList();
                listaEstadoDTO = CatEstadoCitaDAOaDTO.listaCatEstadoCitaDAOaDTO(listaEstados);

            }
            catch(Exception e) {

                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Entrando en página para ver detalles de la cita del médico: {DateTime.Now.ToString()}");

            }

        }

        
        public IActionResult OnPostCrear()
        {
            return RedirectToPage("/Usuarios/PedirCita");
        }
        public IActionResult OnPostBorrar()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./Borrar");
        }
        public IActionResult OnPostDetalles()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./DetallesCita");

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
