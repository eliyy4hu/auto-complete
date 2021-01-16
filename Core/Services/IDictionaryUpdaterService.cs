using System.Collections.Generic;

namespace Core.Services
{
    public interface IDictionaryUpdaterService
    {
        public void InitDictionary(IEnumerable<string> words);
        public void UpdateDictionary(IEnumerable<string> words);
        public void ClearDictionary();
        public IEnumerable<string> GetMostFrequenciesWords(string prefix, int count);
        public void SaveResult();
    }
}
