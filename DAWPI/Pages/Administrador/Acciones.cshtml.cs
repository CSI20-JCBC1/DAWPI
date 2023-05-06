using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Administrador
{
    [Authorize(Roles = "Admin")]
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
