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

                List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();
                foreach (var estadoCita in listaEstadoCita)
                {
                    if (Cita.EstadoCita == estadoCita.EstadoCita)
                    {
                        Cita.EstadoCita = estadoCita.DescEstadoCita;
                    }
                }

                List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();
                foreach (var sala in listaSalaCita)
                {
                    if (Cita.CodSala == sala.CodSala)
                    {
                        Cita.CodSala = sala.NombreSala;
                    }
                }

                List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();
                foreach (var planta in listaPlantaCita)
                {
                    if (Cita.CodPlanta == planta.CodPlanta)
                    {
                        Cita.CodPlanta = planta.NombrePlanta;
                    }
                }


            }
        }
    }
}
