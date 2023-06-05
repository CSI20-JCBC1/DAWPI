using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DatabasePiContext _db;
        public string EmailUsuario { get; set; }

        public IndexModel(ILogger<IndexModel> logger, DatabasePiContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult OnGet()
        {
            EmailUsuario = User.FindFirst("EmailUsuario")?.Value;

            try
            {
                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                if (User.Identity.IsAuthenticated && usuario != null)
                {
                    if (usuario.Rol == 2)
                    {
                        return RedirectToPage("/Usuarios/Citas");
                    }
                    else if (usuario.Rol == 0)
                    {
                        return RedirectToPage("/Administrador/Acciones");
                    }
                    else if (usuario.Rol == 1)
                    {
                        return RedirectToPage("/Medicos/Citas");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString()); // Registra un mensaje informativo en el registro de la aplicación
            }

            return Page();
        }
    }
}