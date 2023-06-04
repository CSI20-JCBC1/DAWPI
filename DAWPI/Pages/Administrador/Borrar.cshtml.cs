using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAL.DAOaDTO;
using DAL.DTO;
using DAL.DTOaDAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace DAWPI.Pages.Administrador
{
    public class BorrarModel : PageModel
    {
        private readonly ILogger<BorrarModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public BorrarModel(DatabasePiContext db, ILogger<BorrarModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        // Propiedades públicas para almacenar los datos del usuario y el detalle de la página
        public UsuarioDTO usuarioDTO { get; set; }
        public int? detalle { get; set; }

        public void OnGet()
        {
            try
            {
                // Registro de evento al entrar en la página
                var message = $"Entrando en página para borrar de usuario o médico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                // Configuración de cabecera de respuesta HTTP
                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";

                detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    // Obtener el usuario o médico a eliminar desde el contexto de base de datos
                    Usuario? usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);

                    if (usuario == null)
                    {
                        // Redireccionar a otra página si el usuario no existe
                        Response.Redirect("/Administrador/Acciones");
                    }
                    else
                    {
                        // Convertir el usuario a un objeto DTO para mostrar en la página
                        usuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                // Registro de excepción en caso de error
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepción en página eliminar usuario o médico de administrador: {DateTime.Now.ToString()}");
            }
        }

        public IActionResult OnPost(string confirmacion)
        {
            try
            {
                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    // Obtener el usuario o médico a eliminar desde el contexto de base de datos
                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);
                    usuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);
                }

                bool citaPendiente = false;

                if (!string.IsNullOrEmpty(confirmacion) && confirmacion.Trim() == usuarioDTO.NombreCompleto)
                {
                    if (usuarioDTO.Rol == 1) // Si es un médico
                    {
                        // Obtener la lista de citas desde el contexto de base de datos
                        List<Cita> listaCitas = _db.Citas.ToList();

                        foreach (Cita cita in listaCitas)
                        {
                            // Verificar si el médico tiene citas pendientes
                            if (cita.NombreMedico == usuarioDTO.NombreCompleto && (cita.EstadoCita.Equals("PFH") || cita.EstadoCita.Equals("A")))
                            {
                                citaPendiente = true;
                                break;
                            }
                        }

                        if (citaPendiente)
                        {
                            // Agregar mensaje de error si hay citas pendientes y redirigir a la página
                            ModelState.AddModelError(string.Empty, "Error, el médico que está intentando borrar tiene citas pendientes. Cuando las termine, podrá ser borrado.");
                            return Page();
                        }

                        // Eliminar médico y su información adicional
                        Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);
                        CatInfoMedico infomedico = _db.CatInfoMedicos.FirstOrDefault(i => i.NombreMedico == usuario.NombreCompleto);

                        if (usuario != null)
                        {
                            _db.Remove(infomedico);
                            _db.Remove(usuario);
                            _db.SaveChanges();

                            // Registro de evento al borrar un médico exitosamente
                            var message = $"Médico borrado con éxito: {DateTime.Now.ToString()}";
                            _logger.LogInformation(message);
                            WriteLogToFile(message);
                        }
                    }
                    else if (usuarioDTO.Rol == 2) // Si es un paciente
                    {
                        // Obtener la lista de citas desde el contexto de base de datos
                        List<Cita> listaCitas = _db.Citas.ToList();
                        Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);

                        foreach (Cita cita in listaCitas)
                        {
                            // Eliminar todas las citas asociadas al paciente
                            if (cita.NombrePaciente == usuario.NombreCompleto)
                            {
                                _db.Remove(cita);
                            }
                        }

                        // Eliminar al paciente
                        _db.Remove(usuario);
                        _db.SaveChanges();

                        // Registro de evento al borrar un paciente exitosamente
                        var message = $"Paciente borrado con éxito: {DateTime.Now.ToString()}";
                        _logger.LogInformation(message);
                        WriteLogToFile(message);
                    }
                }
                else
                {
                    // Agregar mensaje de error si el nombre completo no coincide
                    ModelState.AddModelError(string.Empty, "El nombre completo no coincide. Inténtalo de nuevo.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Registro de excepción en caso de error
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepción al intentar borrar paciente o médico: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Se ha producido un error al intentar borrar el paciente o médico. Por favor, inténtalo nuevamente más tarde.");
                return Page();
            }

            // Redireccionar a la página de acciones después de borrar exitosamente
            return RedirectToPage("/Administrador/Acciones");
        }

        private void WriteLogToFile(string message)
        {
            try
            {
                // Crear el directorio si no existe y escribir el mensaje en el archivo de registro
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));

                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                // Registro de excepción en caso de error al escribir en el archivo de registro
                _logger.LogInformation(ex.ToString());
            }
        }
    }
}
