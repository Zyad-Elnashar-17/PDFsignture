using System.ComponentModel.DataAnnotations;

namespace PDFsignture.ViewModels
{
    public class SignatureVM
    {
        [Required]
        public string SignatureDataUrl { get; set; } // Base64 string from Canvas

        public bool IsDefault { get; set; }
    }
}
