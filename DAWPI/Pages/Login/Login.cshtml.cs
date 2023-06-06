using DAL.DAOaDTO;
using DAL.DTO;
using DAL.Models;
using DAWPI.Pages.Administrador;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAWPI.Pages.Login
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public string EmailUsuario { get; set; }

        [TempData]
        public string MensajeExito { get; set; }

        [BindProperty]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Ingrese un correo electr�nico v�lido.")]
        public string Email { get; set; }

        [BindProperty]
        public string Contrasenia { get; set; }

        public string ControlAcceso { get; set; }
        public List<RolDTO> RolDTO { get; set; }

        private readonly ILogger<LoginModel> _logger;
        private readonly string _logFilePath;
        private readonly DatabasePiContext _db;

        private int MaxFailedAttempts = 5;
        private TimeSpan LockoutDuration = TimeSpan.FromMinutes(30);
        private string FailedAttemptsKey => $"FailedAttempts_{Email}";

        public LoginModel(DatabasePiContext db, ILogger<LoginModel> logger)
        {
            _db = db;
            _logger = logger;
            _logFilePath = @"C:\logs\log.txt";
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            var message = $"Entrando en p�gina para iniciar sesi�n: {DateTime.Now.ToString()}";
            _logger.LogInformation(message);
            WriteLogToFile(message);
            return Page();
        }

        [ActionName("MiLogin")]
        public async Task<IActionResult> OnPostSubmitAsync()
        {
            try
            {
                var failedAttempts = GetFailedAttempts();

                if (failedAttempts >= MaxFailedAttempts)
                {
                    var lockoutEnd = HttpContext.Session.GetString("LockoutEnd");
                    if (!string.IsNullOrEmpty(lockoutEnd) && DateTime.TryParse(lockoutEnd, out var lockoutEndTime))
                    {
                        if (DateTime.Now < lockoutEndTime)
                        {
                            ModelState.AddModelError(string.Empty, "Su cuenta est� bloqueada. Int�ntelo de nuevo m�s tarde.");
                            return Page();
                        }
                        else
                        {
                            SetFailedAttempts(0);
                            HttpContext.Session.Remove("LockoutEnd");
                        }
                    }
                }

                var usuario = _db.Usuarios.FirstOrDefault(e => e.Email == Email);

                if (usuario != null && BCrypt.Net.BCrypt.Verify(Contrasenia, usuario.Contrasenya))
                {
                    SetFailedAttempts(0);

                    if (usuario.Verificado == true)
                    {
                        List<CatRolUsuario> listaRol = _db.CatRolUsuarios.ToList();
                        RolDTO = RolDAOaDTO.listaRolDAOaDTO(listaRol);

                        foreach (RolDTO rol in RolDTO)
                        {
                            if (usuario.Rol == rol.NivelAcceso)
                            {
                                ControlAcceso = rol.ControlAcceso;
                                break;
                            }
                        }

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, usuario.Email),
                            new Claim(ClaimTypes.Role, ControlAcceso),
                            new Claim("EmailUsuario", usuario.Email)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "AuthScheme");
                        await HttpContext.SignInAsync("AuthScheme", new ClaimsPrincipal(claimsIdentity));

                        var message = $"Sesi�n iniciada con �xito: {DateTime.Now.ToString()}";
                        _logger.LogInformation(message);
                        WriteLogToFile(message);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Verifique su usuario para iniciar Sesi�n.");
                        return Page();
                    }
                }
                else
                {
                    failedAttempts++;
                    SetFailedAttempts(failedAttempts);

                    if (failedAttempts >= MaxFailedAttempts)
                    {
                        var lockoutEndTime = DateTime.Now.Add(LockoutDuration);
                        HttpContext.Session.SetString("LockoutEnd", lockoutEndTime.ToString());
                        ModelState.AddModelError(string.Empty, "Su cuenta est� bloqueada. Int�ntelo de nuevo m�s tarde.");
                        return Page();
                    }

                    ModelState.AddModelError(string.Empty, "Error, el usuario con ese email no existe o la contrase�a es incorrecta.");
                    return Page();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                WriteLogToFile($"Excepci�n en la p�gina de inicio de sesi�n: {DateTime.Now.ToString()}");
                ModelState.AddModelError(string.Empty, "Ahora mismo es imposible iniciar la sesi�n. Intentelo m�s tarde.");
                return Page();
            }

            return RedirectToPage("/Index");
        }

        private int GetFailedAttempts()
        {
            return HttpContext.Session.GetInt32(FailedAttemptsKey) ?? 0;
        }

        private void SetFailedAttempts(int count)
        {
            HttpContext.Session.SetInt32(FailedAttemptsKey, count);
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
