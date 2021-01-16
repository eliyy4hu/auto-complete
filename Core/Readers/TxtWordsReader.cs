using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Readers
{
    public class TxtWordsReader : TextReaderBase
    {
        public override List<string> SupportedExtensions => new List<string> { "txt" };


        public override string ReadText(string inputPath)
        {
            using (var fileStream = File.OpenRead(inputPath))
            {
                var array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
                var textFromFile = System.Text.Encoding.UTF8.GetString(array);
                return textFromFile;
            }
        }
    }
}