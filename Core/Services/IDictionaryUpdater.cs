using System.Collections.Generic;

namespace Core.Services
{
    public interface IDictionaryUpdater
    {
        public void InitDictionary(IEnumerable<string> words);

        public void UpdateDictionary(IEnumerable<string> words);

        public void ClearDictionary();

        public void SaveResult();

        public bool Initialized();
    }
}