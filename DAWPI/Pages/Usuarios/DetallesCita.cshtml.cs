using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DAWPI.Pages.Usuarios
{
    [Authorize(Roles = "Paciente")]
    public class DetallesCitaModel : PageModel
    {
        private readonly DatabasePiContext _db;
        public DetallesCitaModel(DatabasePiContext db)
        {
            _db = db;
        }

        public CitaDTO Cita { get; set; }

        public void OnGet(int id)
        {
            Cita cita = _db.Citas.FirstOrDefault(c => c.Id == id);
            CitaDTO citaDTO = CitaDAOaDTO.citaDAOaDTO(cita);
            Cita = citaDTO;
        }
    }
}
