using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Login
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            //Fijamos el logout en nuestro esquema de autenticaci�n y una vez este se efect�a redirigimos a la p�gina de login
            await HttpContext.SignOutAsync("AuthScheme");
            return RedirectToPage("./Login");
        }
    }
}
