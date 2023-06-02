using System;
using System.Collections.Generic;
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

        public UsuarioDTO usuarioDTO { get; set; }
        public int? detalle { get; set; }

        public void OnGet()
        {
            
            try
            {
                var message = $"Entrando en p�gina para borrar de usuario o m�dico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                    Usuario? usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);
                    if (usuario == null)
                    {
                        Response.Redirect("/Administrador/Acciones");
                    }
                    else
                    {
                        usuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepci�n en p�gina eliminar usuario o m�dico de administrador: {DateTime.Now.ToString()}");
            }
        }


        public IActionResult OnPost(string confirmacion)
        {
            try
            {

                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                detalle = HttpContext.Session.GetInt32("detalle");
                Console.WriteLine(detalle);
                if (detalle.HasValue)
                {
                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);
                    usuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);
                }

                bool citaPendiente = false;

                if (!string.IsNullOrEmpty(confirmacion) && confirmacion.Trim() == usuarioDTO.NombreCompleto)
                {
                    // Check if the user is a doctor and has pending appointments
                    if (usuarioDTO.Rol == 1)
                    {
                        List<Cita> listaCitas = _db.Citas.ToList();

                        foreach (Cita cita in listaCitas)
                        {
                            if (cita.NombreMedico == usuarioDTO.NombreCompleto && (cita.EstadoCita.Equals("PFH") || cita.EstadoCita.Equals("A")))
                            {
                                citaPendiente = true;
                                break;
                            }
                        }

                        if (citaPendiente)
                        {
                            ModelState.AddModelError(string.Empty, "Error, el m�dico que est� intentando borrar tiene citas pendientes. Por favor, contacte con �l para que elimine sus asignaciones antes de continuar.");
                            return Page();
                        }

                        Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);
                        CatInfoMedico infomedico = _db.CatInfoMedicos.FirstOrDefault(i => i.NombreMedico == usuario.NombreCompleto);
                        if (usuario != null)
                        {
                            _db.Remove(infomedico);
                            _db.SaveChanges();
                            _db.Remove(usuario);
                            _db.SaveChanges();

                            var message = $"M�dico borrado con �xito: {DateTime.Now.ToString()}";
                            _logger.LogInformation(message);
                            WriteLogToFile(message);

                        }
                    }
                    else if (usuarioDTO.Rol == 2)
                    {
                        List<Cita> listaCitas = _db.Citas.ToList();
                        Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);

                        foreach (Cita cita in listaCitas)
                        {
                            if (cita.NombrePaciente == usuario.NombreCompleto)
                            {
                                _db.Remove(cita);
                            }
                        }

                        _db.SaveChanges();
                        _db.Remove(usuario);
                        _db.SaveChanges();
                        var message = $"Paciente borrado con �xito: {DateTime.Now.ToString()}";
                        _logger.LogInformation(message);
                        WriteLogToFile(message);

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre completo no coincide. Int�ntalo de nuevo.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepci�n al intentar borrar paciente o m�dico: {DateTime.Now.ToString()}");
                return Page();
            }

            return RedirectToPage("/Administrador/Acciones");
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
