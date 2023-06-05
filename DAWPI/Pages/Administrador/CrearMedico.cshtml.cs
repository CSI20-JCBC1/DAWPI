using DAL.DAOaDTO;
using DAL.DTO;
using DAL.DTOaDAO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text.RegularExpressions;

namespace DAWPI.Pages.Administrador
{
    [Authorize(Roles = "Admin")]
    public class CrearMedicoModel : PageModel
    {
        private readonly ILogger<CrearMedicoModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        public CrearMedicoModel(DatabasePiContext db, ILogger<CrearMedicoModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        // Propiedades de vinculaci�n para los campos del formulario
        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Movil { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Especialidad { get; set; }

        [BindProperty]
        public string CodPlanta { get; set; }

        [BindProperty]
        public string CodSala { get; set; }

        [BindProperty]
        public string Contrasenia { get; set; }

        [BindProperty]
        public string Contrasenia2 { get; set; }

        public List<CatSalaDTO> listaSalaCitaDTO { get; set; }
        public List<CatPlantaDTO> listaPlantaCitaDTO { get; set; }

        public void OnGet()
        {
            try
            {
                //Mensaje para que aparezca en nuestro log
                var message = $"Entrando en p�gina para crear m�dico: {DateTime.Now.ToString()}";
                _logger.LogInformation(message);
                WriteLogToFile(message);

                //Mostramos la listaDTO en la vista
                List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();

                listaSalaCitaDTO = CatSalaDAOaDTO.listacatSalaDAOaDTO(listaSalaCita);

                List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();
                listaPlantaCitaDTO = CatPlantaDAOaDTO.listacatPlantaDAOaDTO(listaPlantaCita);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepci�n en la p�gina para crear m�dico: {DateTime.Now.ToString()}");
            }
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            try
            {
                List<CatSalaCitum> listaSalaCita = _db.CatSalaCita.ToList();
                listaSalaCitaDTO = CatSalaDAOaDTO.listacatSalaDAOaDTO(listaSalaCita);

                List<CatPlantaCitum> listaPlantaCita = _db.CatPlantaCita.ToList();
                listaPlantaCitaDTO = CatPlantaDAOaDTO.listacatPlantaDAOaDTO(listaPlantaCita);

                // Crear m�dico en el cat�logo
                Usuario usuario = UsuarioDTOaDAO.usuarioDTOaDAO(new UsuarioDTO(Nombre, Movil, Email, Contrasenia, 1));
                var usuarioBusqueda = _db.Usuarios.FirstOrDefault(e => e.Email == Email);
                string email2 = usuarioBusqueda?.Email ?? string.Empty;
                CatInfoMedico infoMedico = CatInfoMedicoDTOaDAO.catInfoMedicoDTOaDAO(new CatInfoMedicoDTO(Nombre, Especialidad, CodSala, CodPlanta));
                List<CatInfoMedico> catInfoMedicos = _db.CatInfoMedicos.ToList();
                bool salaExistente = false;

                foreach (CatInfoMedico medico in catInfoMedicos)
                {
                    if (medico.CodPlanta == infoMedico.CodPlanta && medico.CodSala == infoMedico.CodSala)
                    {
                        salaExistente = true;
                    }
                }

                // Validaci�n de contrase�as
                // Comprobamos si la contrase�a y su validaci�n coinciden
                // En el formulario ya se controla el n�mero m�ximo de caracteres
                // Validamos tambi�n que la contrase�a contenga al menos una may�scula, una min�scula y un car�cter especial
                //Validamos que el n�mero de telefono sea de 9 d�gitos y que estos sean solo n�meros
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
                string phoneNumberPattern = @"^\d{9}$"; // Expresi�n regular para verificar 9 d�gitos del 0 al 9

                if (Contrasenia != Contrasenia2)
                {
                    ModelState.AddModelError(string.Empty, "Error, las contrase�as no coinciden"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (!Regex.IsMatch(Contrasenia, pattern))
                {
                    ModelState.AddModelError(string.Empty, "Error, la contrase�a debe tener al menos 8 caracteres, incluyendo al menos una may�scula, una min�scula y un car�cter especial"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (!Regex.IsMatch(Movil, phoneNumberPattern))
                {
                    ModelState.AddModelError(string.Empty, "Error, el n�mero de tel�fono debe tener exactamente 9 d�gitos"); // Agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (Email.Equals(email2))
                {
                    ModelState.AddModelError(string.Empty, "El email que quiere registrar ya est� en uso"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (salaExistente)
                {
                    ModelState.AddModelError(string.Empty, "La consulta que desea asignar ya est� en uso, busque otra consulta para este m�dico"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else
                {
                    _db.CatInfoMedicos.Add(infoMedico);
                    _db.SaveChanges();

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Contrasenia);
                    // Asignar la contrase�a encriptada al objeto Usuario
                    usuario.Contrasenya = hashedPassword;
                    usuario.Verificado = true;
                    _db.Usuarios.Add(usuario);
                    _db.SaveChanges();

                    var message = $"M�dico creado: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message);
                    WriteLogToFile(message);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Se ha producido una excepci�n en la p�gina para crear m�dico: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Se produjo un error al crear el m�dico. Por favor, int�ntalo nuevamente m�s tarde.");
                return Page();
            }

            return RedirectToPage("/Administrador/Medicos");
        }

        private void WriteLogToFile(string message)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }
        }
    }
}
