// PROYECTOPDF/Controllers/OperacionesPDFController.cs
using Microsoft.AspNetCore.Mvc;
using NegocioPDF.Repositories;
using System.Security.Claims;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.IO;

namespace PROYECTOPDF.Controllers
{
    public class OperacionesPDFController : Controller
    {
         private readonly OperacionesPDFRepository _operacionesPDFRepository;

        public OperacionesPDFController(OperacionesPDFRepository operacionesPDFRepository)
        {
            _operacionesPDFRepository = operacionesPDFRepository;
        }

        [HttpGet]
        public IActionResult Operaciones()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FusionarPDF()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CortarPDF()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FusionarArchivosPDF(IFormFile archivo1, IFormFile archivo2)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var puedeRealizarOperacion = await _operacionesPDFRepository.RegistrarOperacionPDF(usuarioId, "Fusionar");
            if (!puedeRealizarOperacion)
            {
                TempData["Error"] = "No se puede fusionar: ¡Has excedido el límite de operaciones para tu suscripción básica!";
                return RedirectToAction("FusionarPDF");
            }

            if (archivo1 == null || archivo2 == null || archivo1.Length == 0 || archivo2.Length == 0)
            {
                TempData["Error"] = "Debe seleccionar dos archivos PDF válidos.";
                return RedirectToAction("FusionarPDF");
            }

            try
            {
                var rutaArchivoFusionado = Path.Combine(Path.GetTempPath(), "PDFFusionado.pdf");

                using (var pdf1Stream = archivo1.OpenReadStream())
                using (var pdf2Stream = archivo2.OpenReadStream())
                {
                    var outputDocument = new PdfDocument();
                    var inputDocument1 = PdfReader.Open(pdf1Stream, PdfDocumentOpenMode.Import);
                    CopyPages(inputDocument1, outputDocument);

                    var inputDocument2 = PdfReader.Open(pdf2Stream, PdfDocumentOpenMode.Import);
                    CopyPages(inputDocument2, outputDocument);

                    outputDocument.Save(rutaArchivoFusionado);
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(rutaArchivoFusionado);
                return File(fileBytes, "application/pdf", "PDFFusionado.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al fusionar los archivos PDF: {ex.Message}";
                return RedirectToAction("FusionarPDF");
            }
        }

        private void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CortarArchivoPDF(string rutaArchivoTemp, int startPage, int endPage)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var puedeRealizarOperacion = await _operacionesPDFRepository.RegistrarOperacionPDF(usuarioId, "Cortar");
            if (!puedeRealizarOperacion)
            {
                TempData["Error"] = "No se puede cortar: ¡Has excedido el límite de operaciones para tu suscripción básica!";
                return RedirectToAction("CortarPDF");
            }

            if (string.IsNullOrEmpty(rutaArchivoTemp) || !System.IO.File.Exists(rutaArchivoTemp))
            {
                TempData["Error"] = "Debe seleccionar un archivo PDF válido.";
                return RedirectToAction("CortarPDF");
            }

            try
            {
                var rutaArchivoCortado = Path.Combine(Path.GetTempPath(), "PDFCortado.pdf");

                var inputDocument = PdfReader.Open(rutaArchivoTemp, PdfDocumentOpenMode.Import);
                var outputDocument = new PdfDocument();

                if (startPage < 1 || endPage > inputDocument.PageCount || startPage > endPage)
                {
                    TempData["Error"] = "El rango de páginas es inválido.";
                    return RedirectToAction("CortarPDF");
                }

                for (int pageIndex = startPage; pageIndex <= endPage; pageIndex++)
                {
                    outputDocument.AddPage(inputDocument.Pages[pageIndex - 1]);
                }

                outputDocument.Save(rutaArchivoCortado);

                var fileBytes = await System.IO.File.ReadAllBytesAsync(rutaArchivoCortado);
                return File(fileBytes, "application/pdf", "PDFCortado.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cortar el archivo PDF: {ex.Message}";
                return RedirectToAction("CortarPDF");
            }
        }

        [HttpPost]
        public IActionResult ObtenerTotalPaginas(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return Json(new { success = false, error = "Debe seleccionar un archivo PDF válido." });
            }

            try
            {
                using (var pdfStream = pdfFile.OpenReadStream())
                {
                    var inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
                    var totalPages = inputDocument.PageCount;

                    return Json(new { success = true, totalPaginas = totalPages });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"Error al obtener el número de páginas: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult CargarArchivoTemporal(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return Json(new { success = false, error = "Debe seleccionar un archivo PDF válido." });
            }

            try
            {
                var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    pdfFile.CopyTo(stream);
                }

                var inputDocument = PdfReader.Open(tempFilePath, PdfDocumentOpenMode.Import);
                var totalPages = inputDocument.PageCount;

                return Json(new { success = true, totalPaginas = totalPages, rutaArchivoTemp = tempFilePath });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"Error al cargar el archivo: {ex.Message}" });
            }
        }
    }
}