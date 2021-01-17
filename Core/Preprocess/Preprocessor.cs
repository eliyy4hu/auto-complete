using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.PreProcess
{
    public class Preprocessor : IWordsPreprocessor
    {
        public List<string> PreprocessWords(List<string> words)
        {
            var result = new List<string>();
            foreach (var item in words)
            {
                var current = item.ToLower();
                if (current.Length >= 3)
                {
                    result.Add(current);
                }
            }
            return result;
        }
    }
}
