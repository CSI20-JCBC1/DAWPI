using DAL.DAOaDTO;
using DAL.DTO;
using DAL.DTOaDAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Administrador
{
    public class BorrarModel : PageModel
    {

        private readonly DatabasePiContext _db;
        public BorrarModel(DatabasePiContext db)
        {
            _db = db;
        }
        public UsuarioDTO usuarioDTO { get; set; }
        public int ? detalle { get; set; }
        public void OnGet()
        {
            try
            {

                detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {

                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);

                    usuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public IActionResult OnPost(string confirmacion)
        {

            try {
                detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {

                    Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);

                    usuarioDTO = UsuarioDAOaDTO.usuarioDAOaDTO(usuario);
                }
                bool citaPendiente = false;
                Console.WriteLine(usuarioDTO.NombreCompleto);
                if (!string.IsNullOrEmpty(confirmacion) && confirmacion.Trim() == usuarioDTO.NombreCompleto)
                {
                    // Eliminar el usuario
                    int? detalle = HttpContext.Session.GetInt32("detalle");
                    if (detalle.HasValue)
                    {
                        Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);
                        if (usuario != null)
                        {
                            if (usuario.Rol == 1)
                            {
                                List<Cita> listaCitas = _db.Citas.ToList();
                                if (listaCitas.Count > 0)
                                    foreach (Cita cita in listaCitas)
                                    {
                                        if (cita.NombreMedico.Equals(usuario.NombreCompleto) && (cita.EstadoCita.Equals("PFH") || cita.EstadoCita.Equals("A")))
                                        {
                                            citaPendiente = true;
                                            break;
                                        }
                                    }

                                if (citaPendiente)
                                {
                                    ModelState.AddModelError(string.Empty, "Error, el médico que esta intentando borrar tiene citas pendientes, contacte con el para que quite su asignación y se lo comente al usuario"); // Se agrega un mensaje de error al modelo de estado
                                    return Page();
                                }
                                else
                                {
                                    _db.Remove(usuario);
                                    _db.SaveChanges();
                                }

                            }
                        }
                    }
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre completo no coincide. Inténtalo de nuevo.");
                    return Page();
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToPage("/Administrador/Medicos");
        }

    }
}
