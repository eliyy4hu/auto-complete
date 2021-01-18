using Core.Readers;
using System.Collections.Generic;
using System.Linq;

namespace Core.TextProviders
{
    public class FileTextProvider : ITextProvider
    {
        private readonly TextReaderBase reader;
        private readonly IEnumerable<string> files;

        public FileTextProvider(TextReaderBase reader, IEnumerable<string> files)
        {
            this.reader = reader;
            this.files = files;
        }

        public IEnumerable<string> GetTexts()
        {
            var supportedFiles = files
                .Where(reader.IsSupportedFileExtension);
            return supportedFiles.Select(reader.ReadText);
        }

        private List<string> GetEveryN(List<string> files, int n)
        {
            var result = new List<string>();
            for (var i = 0; i < files.Count; i += n)
            {
                result.Add(files[i]);
            }
            return result;
        }
    }
}