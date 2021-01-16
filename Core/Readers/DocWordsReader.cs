using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Readers
{
    public class DocWordsReader : TextReaderBase
    {
        public override List<string> SupportedExtensions => new List<string>
        {
            "doc",
            "fb2",
            "epub"
        };

        public override string ReadText(string inputPath)
        {
            var doc = Xceed.Words.NET.DocX.Load(inputPath);
            return doc.Text;
        }
    }
}