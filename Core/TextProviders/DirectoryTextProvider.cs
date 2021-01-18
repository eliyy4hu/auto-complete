using Core.Readers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.TextProviders
{
    public class DirectoryTextProvider : ITextProvider
    {
        private readonly FileTextProvider fileTextProvider;

        public DirectoryTextProvider(string directory, TextReaderBase textReader)
        {
            var files = GetFilesRecursive(directory);
            fileTextProvider = new FileTextProvider(textReader, files);
        }

        public IEnumerable<string> GetTexts()
        {
            return fileTextProvider.GetTexts();
        }

        private List<string> GetFilesRecursive(string directory)
        {
            var result = new List<string>();
            try
            {
                foreach (var d in Directory.GetDirectories(directory))
                {
                    foreach (var f in Directory.GetFiles(d))
                    {
                        result.Add(f);
                    }
                    result.AddRange(GetFilesRecursive(d));
                }
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return result;
        }
    }
}