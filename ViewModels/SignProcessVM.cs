namespace PDFsignture.ViewModels
{
    public class SignProcessVM
    {
        public int DocumentId { get; set; }
        public int SignatureId { get; set; }
        public string Position { get; set; } // "Top" or "Bottom"
        public bool SignAllPages { get; set; }
        public string SpecificPages { get; set; } // e.g., "1,3,5"
    }
}
