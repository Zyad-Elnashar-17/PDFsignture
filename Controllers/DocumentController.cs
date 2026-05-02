using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDFsignture.Interfaces;
using PDFsignture.Models;
using PDFsignture.ViewModels;
using System.Security.Claims;

namespace PDFsignture.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IDocumentRepository _docRepo;
        private readonly IWebHostEnvironment _environment;

        public DocumentController(IDocumentRepository docRepo, IWebHostEnvironment environment)
        {
            _docRepo = docRepo;
            _environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var docs = await _docRepo.GetUserDocumentsAsync(userId);
            return View(docs);
        }

        [HttpGet]
        public IActionResult Upload() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(UploadDocumentVM model)
        {
            if (!ModelState.IsValid) return View(model);

            // 1. Backend Validation
            var extension = Path.GetExtension(model.PdfFile.FileName).ToLower();
            if (!UploadDocumentVM.AllowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("PdfFile", "Only PDF files are allowed.");
                return View(model);
            }

            if (model.PdfFile.Length > UploadDocumentVM.MaxFileSize)
            {
                ModelState.AddModelError("PdfFile", "File size cannot exceed 5MB.");
                return View(model);
            }

            // 2. File Saving Logic
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{model.PdfFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.PdfFile.CopyToAsync(stream);
            }

            // 3. Database Save
            var document = new Document
            {
                UserId = userId,
                FileName = model.PdfFile.FileName,
                FilePath = "/uploads/" + uniqueFileName, // Relative path for web access
                UploadedAt = DateTime.UtcNow
            };

            await _docRepo.AddAsync(document);
            await _docRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> ViewPdf(int id)
        {
            var doc = await _docRepo.GetByIdAsync(id);
            if (doc == null || doc.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            return View(doc);
        }


        public async Task<IActionResult> SignedHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // get all signed documents for the user
            var signedDocs = await _docRepo.GetUserSignedDocumentsAsync(userId);
            return View(signedDocs);
        }








    }
}
