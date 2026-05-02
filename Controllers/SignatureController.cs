using Microsoft.AspNetCore.Mvc;
using PDFsignture.Interfaces;
using PDFsignture.Models;
using PDFsignture.Services;
using PDFsignture.ViewModels;
using System.Security.Claims;

namespace PDFsignture.Controllers
{
    public class SignatureController : Controller
    {
        private readonly IDocumentRepository _docRepo;
        private readonly ISignatureRepository _sigRepo;
        private readonly IWebHostEnvironment _environment;

        public SignatureController(IDocumentRepository docRepo, ISignatureRepository sigRepo, IWebHostEnvironment environment)
        {
            _docRepo = docRepo;
            _sigRepo = sigRepo;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var signatures = await _sigRepo.GetUserSignaturesAsync(userId);
            return View(signatures);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignatureVM model)
        {
            if (string.IsNullOrEmpty(model.SignatureDataUrl))
            {
                ModelState.AddModelError("", "Please provide a signature.");
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Convert Base64 to Image File
            var base64Data = model.SignatureDataUrl.Split(',')[1];
            var binData = Convert.FromBase64String(base64Data);

            var fileName = $"sig_{Guid.NewGuid()}.png";
            var folderPath = Path.Combine(_environment.WebRootPath, "signatures");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, binData);

            // 2. Save to DB
            var signature = new Signature
            {
                UserId = userId,
                SignatureImagePath = "/signatures/" + fileName,
                CreatedAt = DateTime.UtcNow,
                IsDefault = model.IsDefault
            };

            await _sigRepo.AddAsync(signature);
            await _sigRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Sign(int docId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // retrieve the document to ensure it belongs to the user
            var document = await _docRepo.GetByIdAsync(docId);

            if (document == null || document.UserId != userId)
            {
                return NotFound();
            }

            // set the document path in ViewBag to be used in the view
            ViewBag.DocumentPath = document.FilePath;

            // prepare the model with the DocumentId
            var model = new SignProcessVM
            {
                DocumentId = docId
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> ProcessSigning(SignProcessVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var document = await _docRepo.GetByIdAsync(model.DocumentId);
            var signature = await _sigRepo.GetByIdAsync(model.SignatureId);

            if (document == null || signature == null || document.UserId != userId)
                return NotFound();

            // 1. Prepare Paths
            var sourcePath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            var signedFileName = $"signed_{Guid.NewGuid()}.pdf";
            var signedRelativePath = "/signed/" + signedFileName;
            var destPath = Path.Combine(_environment.WebRootPath, "signed", signedFileName);

            if (!Directory.Exists(Path.GetDirectoryName(destPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(destPath));

            // 2. Parse Pages
            var pagesToSign = new List<int>();
            if (!model.SignAllPages && !string.IsNullOrEmpty(model.SpecificPages))
            {
                pagesToSign = model.SpecificPages.Split(',').Select(int.Parse).ToList();
            }

            // 3. Execute Service (iText7)
            var signerService = new PdfSignerService();
            await signerService.SignDocumentAsync(
                sourcePath,
                destPath,
                Path.Combine(_environment.WebRootPath, signature.SignatureImagePath.TrimStart('/')),
                pagesToSign,
                model.Position);

            // 4. Save to History Table
            var signedDoc = new SignedDocument
            {
                DocumentId = document.Id,
                SignatureId = signature.Id,
                UserId = userId,
                SignedFilePath = signedRelativePath,
                SignedAt = DateTime.UtcNow
            };

            await _docRepo.AddSignedDocumentAsync(signedDoc);
            await _docRepo.SaveChangesAsync();

            return RedirectToAction("Index", "Document");
        }



    }
}
