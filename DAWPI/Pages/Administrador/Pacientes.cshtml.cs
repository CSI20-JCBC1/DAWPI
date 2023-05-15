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
    public class PacientesModel : PageModel
    {
        [BindProperty]
        public int detalle { get; set; }

        private readonly DatabasePiContext _db;
        public PacientesModel(DatabasePiContext db)
        {
            _db = db;
        }
        public List<UsuarioDTO> usuariosDTO { get; set; }

        public void OnGet()
        {

            try
            {

                List<Usuario> usuarios = _db.Usuarios.Where(u => u.Rol == 2).ToList();

                usuariosDTO = UsuarioDAOaDTO.listaUsuarioDAOaDTO(usuarios);



            }
            catch (Exception e)
            {

                Console.WriteLine(e);

            }

        }
        public IActionResult OnPostBorrar()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./Borrar");
        }
    }
}
