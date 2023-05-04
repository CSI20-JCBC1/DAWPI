using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Administrador
{
    public class AccionesModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostPacientes()
        {
            return RedirectToPage("/Administrador/Pacientes");
        }

        public IActionResult OnPostMedicos()
        {
            return RedirectToPage("/Administrador/Medicos");
        }
    }
}
