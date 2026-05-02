using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Document = iText.Layout.Document;


namespace PDFsignture.Services
{
    public class PdfSignerService
    {


        public async Task<string> SignDocumentAsync(
            string sourcePath,
            string destPath,
            string signaturePath,
            List<int> pages,
            string position)
        {
            return await Task.Run(() =>
            {
                using (PdfReader reader = new PdfReader(sourcePath))
                using (PdfWriter writer = new PdfWriter(destPath))
                using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                {
                    Document document = new Document(pdfDoc);
                    ImageData imageData = ImageDataFactory.Create(signaturePath);
                    Image signatureImage = new Image(imageData).SetWidth(100); // Standard signature size

                    int numberOfPages = pdfDoc.GetNumberOfPages();

                    // Loop through pages (1-indexed in iText)
                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        if (pages.Contains(i) || pages.Count == 0) // Count == 0 means sign all
                        {
                            // Define Position
                            if (position.ToLower() == "top")
                                signatureImage.SetFixedPosition(i, 450, 750); // X, Y coordinates
                            else
                                signatureImage.SetFixedPosition(i, 450, 50);

                            document.Add(signatureImage);
                        }
                    }
                    document.Close();
                }
                return destPath;
            });
        }


    }
}
