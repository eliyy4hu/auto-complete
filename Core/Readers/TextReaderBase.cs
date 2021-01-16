using System.Collections.Generic;
using System.Linq;

namespace Core.Readers
{
    public abstract class TextReaderBase
    {
        public abstract string ReadText(string inputPath);
        public abstract List<string> SupportedExtensions { get; }

        public bool IsSupportedFileExtension(string filename)
        {
            return SupportedExtensions.Contains(GetFileExtension(filename));
        }

        internal string GetFileExtension(string filename)
        {
            return filename.Split('.').Last();
        }
    }
}