using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;
using System.IO;
namespace Core.Readers
{
    public class DocWordsReader : TextReaderBase
    {
        public override List<string> SupportedExtensions => new List<string>
        {
            "doc",
            "docx",
            "fb2",
        };

        public override string ReadText(string inputPath)
        {
            var bytes = File.ReadAllBytes(inputPath);
            MemoryStream memoryStream = new MemoryStream(bytes);

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(memoryStream, false))
            {
                MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                return mainPart.Document.Body.InnerText;
            }
        }
    }
}