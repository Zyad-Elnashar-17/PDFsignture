using System.ComponentModel.DataAnnotations;

namespace PDFsignture.Models
{
    public class SignedDocument
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DocumentId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int SignatureId { get; set; }
        [Required]
        public string SignedFilePath { get; set; }
        public DateTime SignedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Document Document { get; set; }
        public Signature Signature { get; set; }
    }
}
