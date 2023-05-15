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
namespace DAWPI.Pages.Register
{
    public class RegisterModel : PageModel
    {
        private readonly DatabasePiContext _db;
        public RegisterModel(DatabasePiContext db)
        {
            _db = db;
        }

        [BindProperty]
        public string nombre { get; set; }
        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string movil { get; set; }
        [BindProperty]
        public string contrasenia { get; set; }
        [BindProperty]
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
                // Validación de contraseñas
                // Comprobamos si la contraseña y su validación coinciden
                // En el formulario ya se controla el número máximo de caracteres
                // Validamos también que la contraseña contenga al menos una mayúscula, una minúscula y un carácter especial
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
                if (contrasenia != contrasenia2)
                {
                    ModelState.AddModelError(string.Empty, "Error, las contraseñas no coinciden"); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                else if (!Regex.IsMatch(contrasenia, pattern))
                {
                    ModelState.AddModelError(string.Empty, "Error, la contraseña debe tener al menos 8 caracteres, incluyendo al menos una mayúscula, una minúscula, y un carácter especial."); // Se agrega un mensaje de error al modelo de estado
                    return Page();

                }
                //Validamos que el email introducido no se encuentra en la base de datos, si se encuentra mandamos un menasje de error 
                //a la página de registro
                else if (email.Equals(email2))
                {
                    ModelState.AddModelError(string.Empty, "El email que quiere registrar ya está en uso."); // Se agrega un mensaje de error al modelo de estado
                    return Page();
                }
                //Una vez validamos lo anterior, insertamos el usuario en la base de datos.
                else
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(contrasenia);
                    // Asignar la contraseña encriptada al objeto Usuario
                    usuario.Contrasenya = hashedPassword;
                    _db.Usuarios.Add(usuario);
                    _db.SaveChanges();

                    //// Envío de correo de verificación
                    //var message = new MimeMessage();
                    //message.From.Add(new MailboxAddress("Clínica Juan Carlos Bada", "jccloud0128@gmail.com")); // Remitente
                    //message.To.Add(new MailboxAddress(usuario.NombreCompleto, email)); // Destinatario
                    //message.Subject = "Verificación de correo electrónico";

                    //// Cuerpo del mensaje
                    //message.Body = new TextPart("plain")
                    //{
                    //    Text = "¡Gracias por registrarte! Por favor, verifica tu dirección de correo electrónico haciendo clic en el siguiente enlace: https://www.tuweb.com/verificar?codigo=123456"
                    //};

                    //// Configuración del cliente SMTP
                    //using (var client = new SmtpClient())
                    //{
                    //    await client.ConnectAsync("ajcbadcar@iesmurillo.es", 587, false);
                    //    await client.AuthenticateAsync("jccloud0128@gmail.com", "Juancarbc2001#@");
                    //    await client.SendAsync(message);
                    //    await client.DisconnectAsync(true);
                    //}

                }
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.Message);
            }
            return RedirectToPage("../Login/Login");
        }
    }

    
}