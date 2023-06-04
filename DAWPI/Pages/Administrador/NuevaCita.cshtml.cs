using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Linq;

namespace DAWPI.Pages.Administrador
{
    [Authorize(Roles = "Admin")]
    public class NuevaCitaModel : PageModel
    {
        [BindProperty]
        public int idAsig { get; set; }

        private readonly ILogger<NuevaCitaModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public NuevaCitaModel(DatabasePiContext db, ILogger<NuevaCitaModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public string especialidad { get; set; }
        public string nombre { get; set; }
        public CatInfoMedicoDTO infoMedicoDTO { get; set; }
        public List<CitaDTO> listaCitasDTO { get; set; }
        public int IdUsuario { get; set; }

        public void OnGet()
        {
            try
            {
                // Registra la entrada en la p�gina en los logs
                var message = $"Entrando en p�gina para asignar cita a un m�dico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Obtiene el valor de la sesi�n "detalle"
                int? detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    int id = (int)detalle;
                    IdUsuario = id;

                    // Obtiene el usuario correspondiente al id
                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);

                    CatInfoMedico infoMedico = new CatInfoMedico();
                    List<CatInfoMedico> infoMedicos = new List<CatInfoMedico>();
                    infoMedicos = _db.CatInfoMedicos.ToList();
                    foreach (var info in infoMedicos)
                    {
                        if (info.NombreMedico == usuario.NombreCompleto)
                        {
                            infoMedico = info;
                        }
                    }

                    // Asigna los valores de especialidad y nombre del m�dico
                    especialidad = infoMedico.Especialidad;
                    nombre = infoMedico.NombreMedico;

                    // Obtiene la lista de citas que no tienen m�dico asignado
                    List<Cita> listaCitas = _db.Citas.Where(c => c.NombreMedico == null).ToList();

                    // Convierte la lista de citas a una lista de DTOs
                    listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(listaCitas);

                    // Obtiene la lista de estados de cita
                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();

                    // Asigna el nombre descriptivo del estado de cita a cada cita en la lista
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
            catch (Exception ex)
            {
                // Registra la excepci�n en los logs
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepci�n producida en la p�gina para asignar cita a un m�dico: {DateTime.Now.ToString()}");
            }
        }

        public IActionResult OnPostAsignar(int IdUsuario)
        {
            try
            {
                // Obtiene la cita correspondiente al idAsig
                Cita cita = _db.Citas.FirstOrDefault(c => c.Id == idAsig);

                int idUsuario = IdUsuario;

                // Obtiene el usuario correspondiente al idUsuario
                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

                // Obtiene la informaci�n del m�dico correspondiente al usuario
                CatInfoMedico infoMedico = _db.CatInfoMedicos.FirstOrDefault(i => i.NombreMedico == usuario.NombreCompleto);

                if (cita != null)
                {
                    // Actualiza el campo nombreMedico de la cita
                    cita.NombreMedico = usuario.NombreCompleto;

                    cita.CodSala = infoMedico.CodSala;

                    cita.CodPlanta = infoMedico.CodPlanta;

                    cita.EstadoCita = "PFH";

                    // Guarda los cambios en la base de datos
                    _db.SaveChanges();

                    // Registro exitoso en los logs
                    var message = $"Cita asignada con �xito: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message);
                    WriteLogToFile(message);
                }
            }
            catch (Exception ex)
            {
                // Registra la excepci�n en los logs y muestra un mensaje de error
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepci�n producida en la p�gina para asignar cita a un m�dico: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Se produjo un error al asignar la cita. Por favor, intenta nuevamente m�s tarde.");
                return Page();
            }

            // Redirige a la p�gina de "Medicos" despu�s de asignar la cita
            return RedirectToPage("/Administrador/Medicos");
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
