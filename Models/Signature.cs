using System.ComponentModel.DataAnnotations;

namespace PDFsignture.Models
{
    public class Signature
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string SignatureImagePath { get; set; } // Path to saved PNG
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //  Navigation Property
        public ICollection<SignedDocument> SignedDocuments { get; set; }
    }
}
