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

        // Propiedades de vinculación para los campos del formulario
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
                var message = $"Entrando en página para crear médico: {DateTime.Now.ToString()}";
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
                WriteLogToFile($"Se ha producido una excepción en la página para crear médico: {DateTime.Now.ToString()}");
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

                // Crear médico en el catálogo
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

                // Validación de contraseñas
                // Comprobamos si la contraseña y su validación coinciden
                // En el formulario ya se controla el número máximo de caracteres
                // Validamos también que la contraseña contenga al menos una mayúscula, una minúscula y un carácter especial
                //Validamos que el número de telefono sea de 9 dígitos y que estos sean solo números
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
                string phoneNumberPattern = @"^\d{9}$"; // Expresión regular para verificar 9 dígitos del 0 al 9

                if (Contrasenia != Contrasenia2)
                {
                    ModelState.AddModelError(string.Empty, "Error, las contraseñas no coinciden"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (!Regex.IsMatch(Contrasenia, pattern))
                {
                    ModelState.AddModelError(string.Empty, "Error, la contraseña debe tener al menos 8 caracteres, incluyendo al menos una mayúscula, una minúscula y un carácter especial"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (!Regex.IsMatch(Movil, phoneNumberPattern))
                {
                    ModelState.AddModelError(string.Empty, "Error, el número de teléfono debe tener exactamente 9 dígitos"); // Agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (Email.Equals(email2))
                {
                    ModelState.AddModelError(string.Empty, "El email que quiere registrar ya está en uso"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (salaExistente)
                {
                    ModelState.AddModelError(string.Empty, "La consulta que desea asignar ya está en uso, busque otra consulta para este médico"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else
                {
                    _db.CatInfoMedicos.Add(infoMedico);
                    _db.SaveChanges();

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Contrasenia);
                    // Asignar la contraseña encriptada al objeto Usuario
                    usuario.Contrasenya = hashedPassword;
                    usuario.Verificado = true;
                    _db.Usuarios.Add(usuario);
                    _db.SaveChanges();

                    var message = $"Médico creado: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message);
                    WriteLogToFile(message);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ToString());
                WriteLogToFile($"Se ha producido una excepción en la página para crear médico: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Se produjo un error al crear el médico. Por favor, inténtalo nuevamente más tarde.");
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
