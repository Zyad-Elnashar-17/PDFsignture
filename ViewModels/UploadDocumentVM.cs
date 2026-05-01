using System.ComponentModel.DataAnnotations;

namespace PDFsignture.ViewModels
{
    public class UploadDocumentVM
    {
        [Required(ErrorMessage = "Please select a PDF file")]
        [Display(Name = "Upload PDF Document")]
        public IFormFile PdfFile { get; set; }

        // Logic for backend validation
        public static readonly string[] AllowedExtensions = { ".pdf" };
        public const long MaxFileSize = 3 * 1024 * 1024; // 3 MB
    }
}
