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
                // Validaci�n de contrase�as
                // Comprobamos si la contrase�a y su validaci�n coinciden
                // En el formulario ya se controla el n�mero m�ximo de caracteres
                // Validamos tambi�n que la contrase�a contenga al menos una may�scula, una min�scula y un car�cter especial
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
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

                    //// Env�o de correo de verificaci�n
                    //var message = new MimeMessage();
                    //message.From.Add(new MailboxAddress("Cl�nica Juan Carlos Bada", "jccloud0128@gmail.com")); // Remitente
                    //message.To.Add(new MailboxAddress(usuario.NombreCompleto, email)); // Destinatario
                    //message.Subject = "Verificaci�n de correo electr�nico";

                    //// Cuerpo del mensaje
                    //message.Body = new TextPart("plain")
                    //{
                    //    Text = "�Gracias por registrarte! Por favor, verifica tu direcci�n de correo electr�nico haciendo clic en el siguiente enlace: https://www.tuweb.com/verificar?codigo=123456"
                    //};

                    //// Configuraci�n del cliente SMTP
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