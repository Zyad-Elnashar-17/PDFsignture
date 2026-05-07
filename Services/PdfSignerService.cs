using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using PDFsignture.ViewModels;
using Document = iText.Layout.Document;


namespace PDFsignture.Services
{
    public class PdfSignerService
    {


        public async Task<string> SignDocumentAsync(
        string sourcePath,
        string destPath,
        string sigPath,
        List<int> pagesToSign,
        SignProcessVM model)
        {
            return await Task.Run(() =>
            {
                // using iText7 to sign the PDF
                using (PdfReader reader = new PdfReader(sourcePath))
                using (PdfWriter writer = new PdfWriter(destPath))
                using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                {
                    Document document = new Document(pdfDoc);
                    ImageData imageData = ImageDataFactory.Create(sigPath);
                    Image signatureImage = new Image(imageData);

                    // width and height of the signature image can be adjusted as needed
                    signatureImage.SetWidth(100);

                    // select pages to sign based on the model
                    var pagesToSign = new List<int>();
                    if (model.SignAllPages)
                    {
                        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++) pagesToSign.Add(i);
                    }
                    else if (!string.IsNullOrEmpty(model.SpecificPages))
                    {
                        pagesToSign = model.SpecificPages.Split(',').Select(int.Parse).ToList();
                    }
                    else
                    {
                        pagesToSign.Add(model.SelectedPageNumber > 0 ? model.SelectedPageNumber : 1);
                    }

                    foreach (int pageNum in pagesToSign)
                    {
                        int pageIndex = model.SelectedPageNumber;
                        var page = pdfDoc.GetPage(model.SelectedPageNumber);
                        var pageSize = page.GetPageSize();

                        // calculate the position of the signature based on the model's coordinates and container size
                        // function to convert the coordinates from the model to the PDF coordinate system
                        float ratioX = model.CoordinateX / model.ContainerWidth;
                        float ratioY = model.CoordinateY / model.ContainerHeight;

                        float finalX = ratioX * pageSize.GetWidth();


                        float sigHeight = signatureImage.GetImageScaledHeight();
                        //coordinate system in PDF starts from bottom left, so we need to invert the Y coordinate
                        float finalY = pageSize.GetHeight() - (ratioY * pageSize.GetHeight());

                        float sigWidth = signatureImage.GetImageScaledWidth();


                        signatureImage.SetFixedPosition(pageNum, finalX, finalY);
                        document.Add(signatureImage);
                    }

                    document.Close();
                }
                return destPath;
            });
        }


    }
}
