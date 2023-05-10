using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Administrador
{
    [Authorize(Roles = "Admin")]
    public class DetallesPacienteModel : PageModel
    {
        public void OnGet(int detalles)
        {
        }
    }
}
