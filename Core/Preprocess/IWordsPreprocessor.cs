using System.Collections.Generic;

namespace Core.PreProcess
{
    public interface IWordsPreprocessor
    {
        public List<string> PreprocessWords(List<string> words);
    }
}
