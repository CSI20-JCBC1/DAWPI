using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DAWPI.Pages.Login
{
    [AllowAnonymous] // Permite el acceso a esta p�gina sin necesidad de autenticaci�n
    public class LoginModel : PageModel
    {
        public string EmailUsuario { get; set; }
        private readonly DatabasePiContext _db;

        [TempData]
        public string MensajeExito { get; set; }

        [BindProperty]
        public string Email { get; set; } // Propiedad para el campo de correo electr�nico del usuario

        [BindProperty]
        public string Contrasenia { get; set; } // Propiedad para el campo de contrase�a del usuario

        public string ControlAcceso { get; set; } // Propiedad para el control de acceso del usuario

        public List<RolDTO> RolDTO { get; set; } // Lista de roles del usuario en formato DTO

        public LoginModel(DatabasePiContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        [ActionName("MiLogin")]
        public async Task<IActionResult> OnPostSubmitAsync() // M�todo para manejar el env�o del formulario de inicio de sesi�n
        {
            try
            {
                var usuario = _db.Usuarios.FirstOrDefault(e => e.Email == Email); // Se busca al usuario en la base de datos por su correo electr�nico

                if (usuario != null && BCrypt.Net.BCrypt.Verify(Contrasenia, usuario.Contrasenya)) // Si el usuario existe y la contrase�a es correcta
                {
                    if (usuario.Verificado == true)
                    {
                        List<CatRolUsuario> listaRol = _db.CatRolUsuarios.ToList(); // Se obtiene la lista de roles de usuario desde la base de datos
                        RolDTO = RolDAOaDTO.listaRolDAOaDTO(listaRol); // Se convierte la lista de roles de usuario a formato DTO

                        foreach (RolDTO rol in RolDTO) // Se busca el rol del usuario en la lista de roles en formato DTO
                        {
                            if (usuario.Rol == rol.NivelAcceso) // Si se encuentra el rol del usuario
                            {
                                ControlAcceso = rol.ControlAcceso; // Se obtiene el control de acceso del usuario
                                break;
                            }
                        }

                        var claims = new List<Claim> // Se crea una lista de reclamaciones para la identidad del usuario
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Email), // Se crea una reclamaci�n para el identificador de nombre del usuario
                        new Claim(ClaimTypes.Role, ControlAcceso), // Se crea una reclamaci�n para el rol del usuario
                        new Claim("EmailUsuario", usuario.Email) // Se guarda el email en la sesion.
                    };

                        var claimsIdentity = new ClaimsIdentity(claims, "AuthScheme"); // Se crea un objeto ClaimsIdentity con las reclamaciones
                        await HttpContext.SignInAsync("AuthScheme", new ClaimsPrincipal(claimsIdentity)); // Se inicia sesi�n con el esquema de autenticaci�n "AuthScheme"

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
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Verifique su usuario para iniciar Sesi�n."); // Se agrega un mensaje de error al modelo de estado
                        return Page(); // Se devuelve la p�gina de inicio de sesi�n para mostrar el mensaje de error al usuario
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error, el usuario con ese email no existe o la contrase�a es incorrecta."); // Se agrega un mensaje de error al modelo de estado
                    return Page(); // Se devuelve la p�gina de inicio de sesi�n para mostrar el mensaje de error al usuario
                }

            }
            catch (Exception e)
            {
                // Manejo adecuado de excepciones, como registrar los errores en un archivo de registro o mostrar un mensaje de error al usuario
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "Error en el proceso de inicio de sesi�n. Por favor, int�ntelo de nuevo."); // Se agrega un mensaje de error gen�rico al modelo de estado
                return Page(); // Se devuelve la p�gina de inicio de sesi�n para mostrar el mensaje de error al usuario
            }

            // Si se llega a este punto, significa que el inicio de sesi�n fue exitoso
            return RedirectToPage("/Index"); // Se redirige al usuario a la p�gina de inicio despu�s de iniciar sesi�n

            
        }

    }
}
