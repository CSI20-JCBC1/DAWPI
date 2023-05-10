using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Linq;

namespace DAWPI.Pages.Administrador
{
    [Authorize(Roles = "Admin")]
    public class NuevaCitaModel : PageModel
    {
        [BindProperty]
        public int idAsig { get; set; }
        private readonly DatabasePiContext _db;
        public NuevaCitaModel(DatabasePiContext db)
        {
            _db = db;
        }
        public string especialidad { get; set; }
        public string nombre { get; set; }
        public CatInfoMedicoDTO infoMedicoDTO { get; set; }
        public List<CitaDTO> listaCitasDTO { get; set; }
        public int IdUsuario { get; set; }

        public void OnGet(long detalle)
        {
            try 
            {
                int id = (int)detalle;
                IdUsuario = id;

                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);

                CatInfoMedico infoMedico=new CatInfoMedico();
                List<CatInfoMedico> infoMedicos = new List<CatInfoMedico>();
                infoMedicos = _db.CatInfoMedicos.ToList();
                foreach (var info in infoMedicos)
                {
                    if(info.NombreMedico == usuario.NombreCompleto)
                    {
                        infoMedico = info;
                    }
                }


                especialidad = infoMedico.Especialidad;

                nombre = infoMedico.NombreMedico;

                List<Cita> listaCitas = _db.Citas.Where(c => c.NombreMedico == null).ToList();


                listaCitasDTO = CitaDAOaDTO.listaCitaDAOaDTO(listaCitas);

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
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
        }

        public IActionResult OnPostAsignar(int IdUsuario)
        {

            Cita cita = _db.Citas.FirstOrDefault(c  => c.Id == idAsig);

            int idUsuario = IdUsuario;

            Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

            if (cita != null)
            {
                // Actualizar el campo nombreMedico
                cita.NombreMedico = usuario.NombreCompleto;

                // Guardar los cambios en la base de datos
                _db.SaveChanges();
            }

            return RedirectToPage("/Administrador/Medicos");

        }
    }
}
