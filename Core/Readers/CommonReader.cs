using System.Collections.Generic;
using System.Linq;

namespace Core.Readers
{
    public class CommonReader : TextReaderBase
    {
        public List<TextReaderBase> Readers;

        public CommonReader()
        {
            Readers = new List<TextReaderBase>
            {
                new TxtWordsReader(),
                new DocWordsReader()
            };
        }
        public override string ReadText(string inputPath)
        {
            var reader = Readers
                .FirstOrDefault(r => r.IsSupportedFileExtension(inputPath));
            return reader.ReadText(inputPath);
        }

        public override List<string> SupportedExtensions => 
            Readers.SelectMany(x => x.SupportedExtensions).ToList();
    }
}
