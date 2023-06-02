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
        [BindProperty]
        public int detalle { get; set; }
        public long usuId { get; set; }
        public List<CitaDTO> listaCitasDTO { get; set; }
        public void OnGet(/*int detalle*/)
        {
            try
            {
                var message = $"Entrando en página para gestionar citas del médico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                int? detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);

                    usuId = usuario.Id;

                    List<Cita> listaCitas = _db.Citas.ToList();

                    List<Cita> citasUsuarioSeleccionado = listaCitas.Where(c => c.NombreMedico == usuario.NombreCompleto).ToList();

                    listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(citasUsuarioSeleccionado);

                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();

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
                WriteLogToFile($"Se ha producido una excepción la página para gestionar citas del médico: {DateTime.Now.ToString()}");

            }


        }

        public IActionResult OnPostNuevaCita()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./NuevaCita");

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
