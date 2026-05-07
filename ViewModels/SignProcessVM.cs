namespace PDFsignture.ViewModels
{
    public class SignProcessVM
    {
        public int DocumentId { get; set; }
        public int SignatureId { get; set; }
        public string Position { get; set; } // "Top" or "Bottom"
        public bool SignAllPages { get; set; }
        public string SpecificPages { get; set; } // e.g., "1,3,5"

        // pixel coordinates of the signature on the web page (for Anywhere mode)
        public float CoordinateX { get; set; }
        public float CoordinateY { get; set; }

        // dimensions of the container on the web page (necessary for calculating ratios)
        public float ContainerWidth { get; set; }
        public float ContainerHeight { get; set; }

        // the page number selected by the user for signing in Anywhere mode
        public int SelectedPageNumber { get; set; } = 1;
    }
}
