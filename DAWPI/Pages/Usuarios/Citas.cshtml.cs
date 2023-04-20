using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using DAL.Models;

namespace DAWPI.Pages.Usuarios
{
    [Authorize(Roles = "Paciente")]
    public class CitasModel : PageModel
    {
        public void OnGet()
        {
            String email = TempData["Email"] as string;
            
            List<Cita> list = new List<Cita>();

        }
    }
}
