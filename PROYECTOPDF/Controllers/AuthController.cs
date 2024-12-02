using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using NegocioPDF.Repositories;
using NegocioPDF.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PROYECTOPDF.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;

        public AuthController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string password)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "El correo y la contraseña son requeridos");
                return View();
            }

            var usuario = await _usuarioRepository.Login(correo, password);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Correo)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), 
                    authProperties);

                TempData["Mensaje"] = $"Bienvenido, {usuario.Nombre}!";
                return RedirectToAction("MenuPrincipal", "Suscripcion");
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Mensaje"] = "Has cerrado sesión correctamente.";
            return RedirectToAction("Login");
        }
    }
}

    