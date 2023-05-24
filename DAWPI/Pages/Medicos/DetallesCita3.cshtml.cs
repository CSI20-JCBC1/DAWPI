using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Medicos
{
    [Authorize(Roles = "Médico")]
    public class DetallesCita3Model : PageModel
    {
        private readonly DatabasePiContext _db;
        public DetallesCita3Model(DatabasePiContext db)
        {
            _db = db;
        }

        public CitaDTO Cita { get; set; }

        public void OnGet()
        {
            int? detalle = HttpContext.Session.GetInt32("detalle");
            if (detalle.HasValue)
            {
                Cita cita = _db.Citas.FirstOrDefault(c => c.Id == detalle);
                CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
                Cita = citaDTO;
            }
        }
    }
}
