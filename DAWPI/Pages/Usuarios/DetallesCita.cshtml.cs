using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Usuarios
{
    [Authorize(Roles = "Paciente")]
    public class DetallesCitaModel : PageModel
    {
        private readonly ILogger<DetallesCitaModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public DetallesCitaModel(DatabasePiContext db, ILogger<DetallesCitaModel> logger)
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
                var message = $"Entrando en página para ver detalles de la cita del paciente: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                int? detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    Cita cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    Cita = citaDTO;

                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();
                    foreach (var estadoCita in listaEstadoCita)
                    {
                        if (Cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            Cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }

                    List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();
                    foreach (var sala in listaSalaCita)
                    {
                        if (Cita.CodSala == sala.CodSala)
                        {
                            Cita.CodSala = sala.NombreSala;
                        }
                    }

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

                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepción en la  detalles de la cita del usuario: {DateTime.Now.ToString()}");
            }
        }

        //Descargar cita en pdf
        public IActionResult OnPostDescargarDetallesCitaPDF()
        {

            try
            {
                
                int? detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    Cita cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    Cita = citaDTO;

                    List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();
                    foreach (var estadoCita in listaEstadoCita)
                    {
                        if (Cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            Cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }

                    List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();
                    foreach (var sala in listaSalaCita)
                    {
                        if (Cita.CodSala == sala.CodSala)
                        {
                            Cita.CodSala = sala.NombreSala;
                        }
                    }

                    List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();
                    foreach (var planta in listaPlantaCita)
                    {
                        if (Cita.CodPlanta == planta.CodPlanta)
                        {
                            Cita.CodPlanta = planta.NombrePlanta;
                        }
                    }

                }

                // Crea un nuevo documento PDF
                Document document = new Document();

                // Genera un nombre de archivo temporal
                string nombreArchivo = Path.GetTempFileName();

                // Crea un escritor de PDF para generar el documento
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(nombreArchivo, FileMode.Create));

                // Abre el documento
                document.Open();

                // Agrega el contenido al documento
                AgregarContenido(document, Cita);

                // Cierra el documento
                document.Close();

                string nombre = Cita.NombrePaciente;
                // Descarga el archivo PDF generado
                byte[] fileBytes = System.IO.File.ReadAllBytes(nombreArchivo);
                return File(fileBytes, "application/pdf", $"DetallesCita{DateTime.Now.ToShortDateString()}.pdf");

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Excepción a la hora de descargar los detalles de la cita: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "No es posible descargar el pdf con los detalles de su cita intentelo más tarde.");
                return Page();
            }
           
        }

        private void AgregarContenido(Document document, CitaDTO cita)
        {

            // Agrega el contenido al documento PDF utilizando iTextSharp

            // Crea una fuente para el título
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);

            // Crea una fuente para los subtítulos
            Font subtituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK);

            // Crea una fuente para el contenido
            Font contenidoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.GRAY);

            // Crea un párrafo para el título
            Paragraph titulo = new Paragraph("Detalles de la cita", tituloFont);
            titulo.Alignment = Element.ALIGN_CENTER;
            titulo.SpacingAfter = 20;

            // Agrega el título al documento
            document.Add(titulo);

            // Agrega los detalles de la cita al documento
            AgregarDetalle(document, "Asunto", cita.Asunto, subtituloFont, contenidoFont);
            AgregarDetalle(document, "Nombre del paciente", cita.NombrePaciente, subtituloFont, contenidoFont);
            AgregarDetalle(document, "Síntomas", cita.Sintomas, subtituloFont, contenidoFont);

            if (!cita.EstadoCita.Equals("Pendiente de asignación"))
            {
                AgregarDetalle(document, "Médico", cita.NombreMedico, subtituloFont, contenidoFont);

                if (cita.EstadoCita.Equals("Finalizada") || cita.EstadoCita.Equals("Asignada"))
                {
                    AgregarDetalle(document, "Fecha", cita.Fecha.ToString(), subtituloFont, contenidoFont);
                    AgregarDetalle(document, "Hora", cita.Hora.ToString(), subtituloFont, contenidoFont);
                    AgregarDetalle(document, "Planta", cita.CodPlanta, subtituloFont, contenidoFont);
                    AgregarDetalle(document, "Consulta", cita.CodSala, subtituloFont, contenidoFont);
                }

                if (cita.EstadoCita.Equals("Finalizada"))
                {
                    AgregarDetalle(document, "Enfermedad diagnosticada", cita.Enfermedad, subtituloFont, contenidoFont);
                    AgregarDetalle(document, "Plan de tratamiento y solución", cita.Solucion, subtituloFont, contenidoFont);
                }
            }

            AgregarDetalle(document, "Estado de la cita", cita.EstadoCita, subtituloFont, contenidoFont);
        }

        private void AgregarDetalle(Document document, string titulo, string contenido, Font subtituloFont, Font contenidoFont)
        {
            // Crea un párrafo para el subtítulo
            Paragraph subtitulo = new Paragraph(titulo + ":", subtituloFont);
            subtitulo.SpacingBefore = 10;

            // Crea un párrafo para el contenido
            Paragraph contenidoParagraph = new Paragraph(contenido, contenidoFont);
            contenidoParagraph.SpacingAfter = 10;

            // Agrega el subtítulo y el contenido al documento
            document.Add(subtitulo);
            document.Add(contenidoParagraph);
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
