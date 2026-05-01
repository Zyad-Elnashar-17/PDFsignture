namespace PDFsignture.Models
{
    public class Signature
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SignatureImagePath { get; set; } // Path to saved PNG
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
