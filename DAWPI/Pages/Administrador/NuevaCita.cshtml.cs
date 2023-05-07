using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DAWPI.Pages.Administrador
{
    public class NuevaCitaModel : PageModel
    {
        private readonly DatabasePiContext _db;
        public NuevaCitaModel(DatabasePiContext db)
        {
            _db = db;
        }
        public CatInfoMedicoDTO infoMedicoDTO { get; set; }

        public void OnGet(long detalle)
        {
            try 
            {
                int id = (int)detalle;
                Usuario usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);

                CatInfoMedico infoMedico = _db.CatInfoMedicos.FirstOrDefault(m => m.NombreMedico == usuario.NombreCompleto);
                infoMedicoDTO = CatInfoMedicoDAOaDTO.catInfoMedicoDAOaDTO(infoMedico);



            } 
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
