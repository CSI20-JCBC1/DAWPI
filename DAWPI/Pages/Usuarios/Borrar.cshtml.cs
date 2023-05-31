using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Usuarios
{
    public class BorrarModel : PageModel
    {
        private readonly DatabasePiContext _db;
        public BorrarModel(DatabasePiContext db)
        {
            _db = db;
        }

        public CitaDTO citaDTO { get; set; }
        public int? detalle { get; set; }

        public void OnGet()
        {
            try
            {
                HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                detalle = HttpContext.Session.GetInt32("detalle");
                if (detalle.HasValue)
                {
                   Cita? cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    if (cita == null)
                    {
                        Response.Redirect("/Administrador/Medicos");
                    }
                    else
                    {
                        citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                    Cita? cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    if (cita == null)
                    {
                        Response.Redirect("/Administrador/Medicos");
                    }
                    else
                    {
                        citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                    }
                }

                bool citaPendiente = false;

                if (!string.IsNullOrEmpty(confirmacion) && confirmacion.Trim() == citaDTO.Asunto)
                {
                    Cita? cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                    _db.Remove(cita);
                    _db.SaveChanges();


                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre completo no coincide. Inténtalo de nuevo.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToPage("/Usuarios/Citas");
        }
    }
}
