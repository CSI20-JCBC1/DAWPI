using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MailKit.Net.Smtp;
using DAL.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace DAWPI.Pages.Login
{
    public class RestaurarContraseñaModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        private readonly ILogger<RestaurarContraseñaModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;
        public RestaurarContraseñaModel(DatabasePiContext db, ILogger<RestaurarContraseñaModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            // Validar el correo electrónico
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError(string.Empty, "El correo electrónico es requerido.");
                return Page();
            }


            try
            {
                var usuario = _db.Usuarios.FirstOrDefault(e => e.Email == Email); // Se busca al usuario en la base de datos por su correo electrónico

                if (usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "El correo del que quiere recuperar la contraseña no está registrado en nuestra página web.");
                    return Page();
                }
                else
                {
                    // Envío de correo de verificación
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Clínica Juan Carlos Bada", "jccloud0128@gmail.com")); // Remitente
                    message.To.Add(new MailboxAddress(usuario.NombreCompleto, Email)); // Destinatario
                    message.Subject = "Verificación de correo electrónico";

                    // Generar un código de cambio de contraseña de 8 caracteres
                    string code = GenerarCodigo(8);


                    // Cuerpo del mensaje
                    message.Body = new TextPart("html")
                    {
                        Text = $"¡Hola! Este correo es para la solicitud de cambio de contraseña. Para poder cambiar la contraseña, introduzca el siguiente código: <strong><span style=\"background-color: yellow;\">{code}</span></strong>"
                    };

                    // Configuración del cliente SMTP
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync("smtp.gmail.com", 587, false);
                        await client.AuthenticateAsync("jccloud0128@gmail.com", "jxympbzoabvxkzea");
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                    var message1 = $"Correo para reestablecer contraseña enviado con éxito: {DateTime.Now.ToString()}";
                    _logger.LogInformation(message1);
                    WriteLogToFile(message1);

                    HttpContext.Session.SetString("Email", Email);
                    HttpContext.Session.SetString("Code", code);
                    return RedirectToPage("/Login/VerificacionCodigo");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                WriteLogToFile($"Se ha producido una excepción al intentar enviar código de verificación: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Ocurrió un error al enviar el correo electrónico. Por favor, inténtalo de nuevo.");  
                return Page();
            }
            

        }

        public static string GenerarCodigo(int longitud)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[longitud];
                rng.GetBytes(data);

                char[] result = new char[longitud];
                for (int i = 0; i < longitud; i++)
                {
                    byte value = data[i];
                    result[i] = validChars[value % validChars.Length];
                }

                return new string(result);
            }
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
