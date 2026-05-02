using System.ComponentModel.DataAnnotations;

namespace PDFsignture.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }
        [Required]
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<SignedDocument> SignedDocuments { get; set; }
    }
}
