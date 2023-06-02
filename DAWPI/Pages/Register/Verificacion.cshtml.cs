using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Register
{
    public class VerificacionModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Contrasenia { get; set; }
        private readonly DatabasePiContext _db;
        public VerificacionModel(DatabasePiContext db)
        {
            _db = db;
        }
        public IActionResult OnPostVerificar()
        {

            try
            {
                var usuario = new Usuario();
                List<Usuario> listausuarios = _db.Usuarios.ToList();
                bool existe = false;
                foreach (Usuario usu in listausuarios)
                {
                    if (Email == usu.Email)
                    {
                        usuario = _db.Usuarios.FirstOrDefault(e => e.Email == Email);
                        existe = true; break;
                    }
                }


                if (existe)
                {
                    if (usuario.Verificado == true)
                    {
                        ModelState.AddModelError(string.Empty, "Su usuario ya está verificado."); // Se agrega un mensaje de error al modelo de estado
                        return Page(); // Se devuelve la página de inicio de sesión para mostrar el mensaje de error al usuario
                    }

                    

                    if (BCrypt.Net.BCrypt.Verify(Contrasenia, usuario.Contrasenya))
                    {

                        usuario.Verificado = true;
                        _db.SaveChanges();

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Contraseña incorrecta, asegurese de introducir bien su contraseña."); // Se agrega un mensaje de error al modelo de estado
                        return Page(); // Se devuelve la página de inicio de sesión para mostrar el mensaje de error al usuario
                    }

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre de usuario no existe, por favor asegureser de introducir bien su nombre."); // Se agrega un mensaje de error al modelo de estado
                    return Page(); // Se devuelve la página de inicio de sesión para mostrar el mensaje de error al usuario
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            TempData["MensajeExito"] = "Verificación exitosa. Puedes iniciar sesión.";
            return RedirectToPage("../Login/Login");
        }

    }
}

