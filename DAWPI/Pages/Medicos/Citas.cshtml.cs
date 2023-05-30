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
    public class CitasModel : PageModel
    {
        [BindProperty]
        public int detalle { get; set; }
        public string EmailUsuario { get; set; }

        private readonly DatabasePiContext _db;
        public CitasModel(DatabasePiContext db)
        {
            _db = db;
        }

        public List<CitaDTO> listaCitasDTO { get; set; }

        public List<CatEstadoCitaDTO> listaEstadoDTO { get; set; }
        public void OnGet()
        {
            EmailUsuario = User.FindFirst("EmailUsuario")?.Value;

            try
            {

                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Email == EmailUsuario);

                List<Cita> listaCitas = _db.Citas.ToList();

                List<Cita> citasUsuarioSeleccionado = listaCitas.Where(c => c.NombreMedico == usuario.NombreCompleto).ToList();

                listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(citasUsuarioSeleccionado);

                List<CatEstadoCitum> listaEstadoCita = _db.CatEstadoCita.ToList();
                foreach (CitaDTO cita in listaCitasDTO)
                {
                    foreach (var estadoCita in listaEstadoCita)
                    {
                        if (cita.EstadoCita == estadoCita.EstadoCita)
                        {
                            cita.EstadoCita = estadoCita.DescEstadoCita;
                        }
                    }
                }

                List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();
                foreach (CitaDTO cita in listaCitasDTO)
                {
                    foreach (var sala in listaSalaCita)
                    {
                        if (cita.CodSala == sala.CodSala)
                        {
                            cita.CodSala = sala.NombreSala;
                        }
                    }
                }

                List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();
                foreach (CitaDTO cita in listaCitasDTO)
                {
                    foreach (var planta in listaPlantaCita)
                    {
                        if (cita.CodPlanta == planta.CodPlanta)
                        {
                            cita.CodPlanta = planta.NombrePlanta;
                        }
                    }
                }

                List<CatEstadoCitum> listaEstados = _db.CatEstadoCita.ToList();
                listaEstadoDTO = CatEstadoCitaDAOaDTO.listaCatEstadoCitaDAOaDTO(listaEstados);

            }
            catch (Exception e)
            {

                Console.WriteLine(e);

            }

        }

        public IActionResult OnPostDetalles()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./DetallesCita");

        }

        public IActionResult OnPostDetalles2()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./DetallesCita2");

        }

        public IActionResult OnPostDetalles3()
        {

            HttpContext.Session.SetInt32("detalle", detalle);
            return RedirectToPage("./DetallesCita3");

        }


    }
}
