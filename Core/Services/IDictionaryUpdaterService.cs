using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public interface IDictionaryUpdaterService
    {
        public void InitDictionary(IEnumerable<string> words);
        public void UpdateDictionary(IEnumerable<string> words);
        public void ClearDictionary();
    }
}
