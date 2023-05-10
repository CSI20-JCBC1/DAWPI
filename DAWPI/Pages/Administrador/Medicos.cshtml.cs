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
    public class MedicosModel : PageModel
    {
        [BindProperty]
        public int detalle { get; set; }

        private readonly DatabasePiContext _db;
        public MedicosModel(DatabasePiContext db)
        {
            _db = db;
        }

        public List<UsuarioDTO> usuariosDTO { get; set; }

        public void OnGet()
        {

            try
            {

                List<Usuario> usuarios = _db.Usuarios.Where(u => u.Rol == 1).ToList();

                usuariosDTO=UsuarioDAOaDTO.listaUsuarioDAOaDTO(usuarios);

               

            }
            catch (Exception e)
            {

                Console.WriteLine(e);

            }

        }

        public IActionResult OnPostCrear()
        {
            return RedirectToPage("./CrearMedico/");
        }

        public IActionResult OnPostGestionCitas()
        {

            return RedirectToPage("./GestionCitas/", new { detalle = detalle });

        }

        public IActionResult OnPostBorrar()
        {

            return RedirectToPage("./Borrar/", new { detalle = detalle });

        }

    }
}
