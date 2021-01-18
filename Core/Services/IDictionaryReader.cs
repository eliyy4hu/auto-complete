using System.Collections.Generic;

namespace Core.Services
{
    public interface IDictionaryReader
    {
        public IEnumerable<string> GetMostFrequenciesWords(string prefix, int count);
    }
}