using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Login
{
    public class AccesoDenegadoModel : PageModel
    {
        public IActionResult Index()
        {
            return RedirectToPage("./AccesoDenegado");
        }
    }
}
