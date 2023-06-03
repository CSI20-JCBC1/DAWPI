using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MailKit.Net.Smtp;

namespace DAWPI.Pages.Login
{
    public class RestaurarContraseñaModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validar el correo electrónico (puedes agregar más validaciones si es necesario)
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError(string.Empty, "El correo electrónico es requerido.");
                return Page();
            }

            try
            {
                // Crear el mensaje de correo electrónico
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Nombre Remitente", "remite@example.com")); // Remitente
                message.To.Add(new MailboxAddress("Nombre Destinatario", Email)); // Destinatario
                message.Subject = "Restauración de contraseña"; // Asunto del correo

                // Contenido del correo
                message.Body = new TextPart("plain")
                {
                    Text = "Hola, has solicitado restaurar tu contraseña. Puedes hacerlo siguiendo el enlace: <a href='https://example.com/restaurar'>Restaurar contraseña</a>"
                };

                // Configurar el cliente SMTP
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.example.com", 587, false); // Configurar el servidor SMTP y el puerto
                    await client.AuthenticateAsync("usuario@example.com", "contraseña"); // Configurar las credenciales de autenticación
                    await client.SendAsync(message); // Enviar el mensaje de correo electrónico
                    await client.DisconnectAsync(true); // Desconectar el cliente SMTP
                }

                return RedirectToPage("/Login/Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al enviar el correo electrónico. Por favor, inténtalo de nuevo.");
                // Puedes agregar el registro del error si lo deseas: _logger.LogError(ex, "Error al enviar correo electrónico");
                return Page();
            }
        }
    }
}
