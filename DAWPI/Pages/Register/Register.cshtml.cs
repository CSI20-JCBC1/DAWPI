using DAL.DTO;
using DAL.DTOaDAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using DAWPI.Pages.Medicos;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace DAWPI.Pages.Register
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public RegisterModel(DatabasePiContext db, ILogger<RegisterModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        [BindProperty]
        public string nombre { get; set; }
        [BindProperty]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Ingrese un correo electr�nico v�lido ...@....")]
        public string email { get; set; }
        [BindProperty]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El n�mero de tel�fono debe tener exactamente 9 d�gitos.")]
        public string movil { get; set; }
        [BindProperty]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "La contrase�a debe tener al menos 8 caracteres, incluyendo al menos una may�scula, una min�scula, y un car�cter especial..")]
        public string contrasenia { get; set; }
        [BindProperty]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "La contrase�a debe tener al menos 8 caracteres, incluyendo al menos una may�scula, una min�scula, y un car�cter especial..")]
        public string contrasenia2 { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostSubmitAsync()
        {
            try
            {
                Usuario usuario = UsuarioDTOaDAO.usuarioDTOaDAO(new UsuarioDTO(nombre, movil, email, contrasenia));
                var usuarioBusqueda = _db.Usuarios.FirstOrDefault(e => e.Email == email);
                string email2 = usuarioBusqueda?.Email ?? string.Empty;
                // Validaci�n de contrase�as
                // Comprobamos si la contrase�a y su validaci�n coinciden
                // En el formulario ya se controla el n�mero m�ximo de caracteres
                // Validamos tambi�n que la contrase�a contenga al menos una may�scula, una min�scula y un car�cter especial
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
                string phoneNumberPattern = @"^\d{9}$"; // Expresi�n regular para verificar 9 d�gitos del 0 al 9

                if (contrasenia != contrasenia2)
                {
                    ModelState.AddModelError(string.Empty, "Error, las contrase�as no coinciden"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (!Regex.IsMatch(contrasenia, pattern))
                {
                    ModelState.AddModelError(string.Empty, "Error, la contrase�a debe tener al menos 8 caracteres, incluyendo al menos una may�scula, una min�scula, y un car�cter especial."); // Se agrega un mensaje de error al modelo de estado
                    return Page();

                }
                else if (!Regex.IsMatch(movil, phoneNumberPattern))
                {
                    ModelState.AddModelError(string.Empty, "Error, el n�mero de tel�fono debe tener exactamente 9 d�gitos."); // Agrega un mensaje de error al modelo de estado
                    return Page();
                }
                //Validamos que el email introducido no se encuentra en la base de datos, si se encuentra mandamos un menasje de error 
                //a la p�gina de registro
                else if (email.Equals(email2))
                {
                    ModelState.AddModelError(string.Empty, "El email que quiere registrar ya est� en uso."); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                //Una vez validamos lo anterior, insertamos el usuario en la base de datos.
                else
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(contrasenia);
                    // Asignar la contrase�a encriptada al objeto Usuario
                    usuario.Contrasenya = hashedPassword;
                    _db.Usuarios.Add(usuario);
                    _db.SaveChanges();

                    // Env�o de correo de verificaci�n
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Cl�nica Juan Carlos Bada", "jccloud0128@gmail.com")); // Remitente
                    message.To.Add(new MailboxAddress(usuario.NombreCompleto, email)); // Destinatario
                    message.Subject = "Verificaci�n de correo electr�nico";

                    string verificationLink = "https://clinicajcbc.juancarlosbada.es/Register/Verificacion"; // // Enlace de verificaci�n


                    // Cuerpo del mensaje
                    message.Body = new TextPart("html")
                    {
                        Text = $"�Gracias por registrarte! Por favor, verifica tu direcci�n de correo electr�nico haciendo <a href=\"{verificationLink}\">click aqu�</a>."
                    };

                    // Configuraci�n del cliente SMTP
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync("smtp.gmail.com", 587, false);
                        await client.AuthenticateAsync("jccloud0128@gmail.com", "jxympbzoabvxkzea");
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                    var message1 = $"Enviado correo de verificaci�n de contras�a: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message1);
                    WriteLogToFile(message1);

                }
            }
            catch (Exception e)
            {
                var message = $"Entrando en p�gina para la gesti�n de pacientes: {DateTime.Now.ToString()}";
                _logger.LogInformation(e.ToString());
                WriteLogToFile(message);
            }
            return RedirectToPage("../Login/Login");
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