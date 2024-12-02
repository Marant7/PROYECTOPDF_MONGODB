// PROYECTOPDF/Controllers/SuscripcionController.cs
using Microsoft.AspNetCore.Mvc;
using NegocioPDF.Models;
using NegocioPDF.Repositories;
using System.Security.Claims;

namespace PROYECTOPDF.Controllers
{
    public class SuscripcionController : Controller
    {
       private readonly DetalleSuscripcionRepository _detalleSuscripcionRepository;

        public SuscripcionController(DetalleSuscripcionRepository detalleSuscripcionRepository)
        {
            _detalleSuscripcionRepository = detalleSuscripcionRepository;
        }

        public async Task<IActionResult> MenuPrincipal()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var detalleSuscripcion = await _detalleSuscripcionRepository.ObtenerPorUsuarioId(usuarioId);

            if (detalleSuscripcion == null)
            {
                TempData["Mensaje"] = "No se encontró la suscripción para el usuario.";
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.NombreUsuario = detalleSuscripcion.Usuario.Nombre;
            ViewBag.TipoSuscripcion = !string.IsNullOrEmpty(detalleSuscripcion.tipo_suscripcion) 
                ? detalleSuscripcion.tipo_suscripcion 
                : "No especificado";
            ViewBag.OperacionesRealizadas = detalleSuscripcion.operaciones_realizadas;
            ViewBag.MaxOperaciones = detalleSuscripcion.tipo_suscripcion.Equals("basico", StringComparison.OrdinalIgnoreCase) ? 5 : (int?)null;

            return View();
        }

        public IActionResult ComprarPremium()
        {
            var fechaInicio = DateTime.Now;
            var fechaFinal = fechaInicio.AddDays(30);

            ViewBag.FechaInicio = fechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFinal = fechaFinal.ToString("yyyy-MM-dd");
            ViewBag.Precio = 50.00m;

            return View();
        }

        [HttpPost]
public async Task<IActionResult> ConfirmarCompra()
{
    try
    {
        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"UsuarioId obtenido: {usuarioId}");

        // Aquí va la verificación que te mencioné
        var suscripcionExistente = await _detalleSuscripcionRepository.ObtenerPorUsuarioId(usuarioId);
        if (suscripcionExistente == null)
        {
            Console.WriteLine("No se encontró suscripción existente, creando nueva...");
        }

        var suscripcion = new DetalleSuscripcion
        {
            UsuarioId = usuarioId,
            tipo_suscripcion = "premium",
            fecha_inicio = DateTime.Now,
            fecha_final = DateTime.Now.AddDays(30),
            precio = 50.00m,
            operaciones_realizadas = 0
        };

        Console.WriteLine("Intentando actualizar suscripción...");
        await _detalleSuscripcionRepository.ActualizarSuscripcion(suscripcion);
        Console.WriteLine("Suscripción actualizada exitosamente");

        TempData["Mensaje"] = "¡Felicitaciones! Has comprado la suscripción Premium.";
        return RedirectToAction("MenuPrincipal");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al actualizar suscripción: {ex.Message}");
        TempData["Error"] = $"Error al procesar la compra: {ex.Message}";
        return RedirectToAction("ComprarPremium");
    }
}
         }
}   





