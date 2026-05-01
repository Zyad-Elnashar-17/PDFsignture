namespace PDFsignture.Models
{
    public class SignedDocument
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string UserId { get; set; }
        public string SignedFilePath { get; set; }
        public DateTime SignedAt { get; set; } = DateTime.Now;
    }
}
