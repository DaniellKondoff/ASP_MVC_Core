using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using LearningSystem.Services.Contracts;
using System.IO;

namespace LearningSystem.Services.Implementations
{
    public class PdfGenerator : IPdfGenerator
    {
        public byte[] GeneratePdfGromHtml(string html)
        {
            var pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var htmlParser = new HtmlWorker(pdfDoc);

            using (var memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                using (var stringReader = new StringReader(html))
                {
                    htmlParser.Parse(stringReader);
                }

                pdfDoc.Close();

                return memoryStream.ToArray();      
            }
        }
    }
}
