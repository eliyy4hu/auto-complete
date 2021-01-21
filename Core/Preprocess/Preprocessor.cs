using System.Collections.Generic;

namespace Core.PreProcess
{
    public class Preprocessor : IWordsPreprocessor
    {
        private const int Min_Word_Length = 3;

        public List<string> PreprocessWords(List<string> words)
        {
            var result = new List<string>();
            foreach (var item in words)
            {
                var current = item.ToLower();
                if (current.Length >= Min_Word_Length)
                {
                    result.Add(current);
                }
            }
            return result;
        }
    }
}