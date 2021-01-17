using System;
using System.Collections.Generic;
using System.Text;

namespace Core.PreProcess
{
    public interface IWordsPreprocessor
    {
        public List<string> PreprocessWords(List<string> words);
    }
}
