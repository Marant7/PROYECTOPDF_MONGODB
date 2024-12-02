using Microsoft.AspNetCore.Mvc; // Para el uso de Controller y IActionResult
using NegocioPDF.Repositories;  // Para UsuarioRepository
using NegocioPDF.Models;        // Para el modelo Usuario



    public class RegistrationController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;

        public RegistrationController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Registrarse(Usuario usuario)
{
    try
    {
        Console.WriteLine($"Intentando registrar usuario: {usuario.Correo}");
        await _usuarioRepository.RegistrarUsuario(usuario);
        Console.WriteLine("Usuario registrado exitosamente");
        TempData["Mensaje"] = "Usuario registrado con éxito.";
        return RedirectToAction("Login", "Auth");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al registrar usuario: {ex.Message}");
        ModelState.AddModelError(string.Empty, ex.Message);
        return View(usuario);
    }
}
    }

