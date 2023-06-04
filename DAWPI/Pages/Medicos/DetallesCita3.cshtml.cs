using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Medicos
{
    [Authorize(Roles = "M�dico")]
    public class DetallesCita3Model : PageModel
    {
        private readonly ILogger<DetallesCita3Model> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public DetallesCita3Model(DatabasePiContext db, ILogger<DetallesCita3Model> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public CitaDTO Cita { get; set; }

        public void OnGet()
        {
            try
            {
                // Registrar entrada en el log
                var message = $"Entrando en p�gina para ver detalles de la cita del m�dico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Obtener el identificador de la cita desde la sesi�n
                int? detalle = HttpContext.Session.GetInt32("detalle");

                if (detalle.HasValue)
                {
                    // Buscar la cita en la base de datos
                    Cita cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);

                    // Convertir la cita a un DTO para su visualizaci�n
                    CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    Cita = citaDTO;

                    // Obtener lista de estados de cita y reemplazar el c�digo por el nombre correspondiente
                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();
                    foreach (var estadoCita in listaEstadoCita)
                    {
                        if (Cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            Cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }

                    // Obtener lista de salas de cita y reemplazar el c�digo por el nombre correspondiente
                    List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();
                    foreach (var sala in listaSalaCita)
                    {
                        if (Cita.CodSala == sala.CodSala)
                        {
                            Cita.CodSala = sala.NombreSala;
                        }
                    }

                    // Obtener lista de plantas de cita y reemplazar el c�digo por el nombre correspondiente
                    List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();
                    foreach (var planta in listaPlantaCita)
                    {
                        if (Cita.CodPlanta == planta.CodPlanta)
                        {
                            Cita.CodPlanta = planta.NombrePlanta;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar la excepci�n en el log
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepci�n en la  detalles de la cita del m�dico: {DateTime.Now.ToString()}");
            }
        }

        private void WriteLogToFile(string message)
        {
            try
            {
                // Crear el directorio para el archivo de log si no existe
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));

                // Escribir el mensaje en el archivo de log
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                // Registrar la excepci�n en el log
                _logger.LogInformation(ex.ToString());
            }
        }
    }
}
