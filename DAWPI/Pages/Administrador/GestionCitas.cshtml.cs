using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Administrador
{
    public class GestionCitasModel : PageModel
    {
        [BindProperty]
        public int detalle { get; set; }

        private readonly DatabasePiContext _db;
        public GestionCitasModel(DatabasePiContext db)
        {
            _db = db;
        }

        public long usuId { get; set; }
        public List<CitaDTO> listaCitasDTO { get; set; }
        public void OnGet(int detalle)
        {
            try
            {

                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == detalle);

                usuId = usuario.Id;

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

            }
            catch (Exception e)
            {

                Console.WriteLine(e);

            }


        }

        public IActionResult OnPostNuevaCita()
        {

            return RedirectToPage("./NuevaCita/", new { detalle = detalle });

        }
    }
}
