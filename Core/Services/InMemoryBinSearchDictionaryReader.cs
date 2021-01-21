using Core.Models;
using Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class InMemoryBinSearchDictionaryReader : IDictionaryReader
    {
        private DataContext context;
        private DictionaryEntry[] dictionary;

        public InMemoryBinSearchDictionaryReader(DataContext context)
        {
            this.context = context;
            dictionary = context.FrequencyDictionary.ToArray();
        }

        public IEnumerable<string> GetMostFrequenciesWords(string prefix, int count)
        {
            var index = Search.IndexOfFirstGreatOrEqual(dictionary, e => e.Word, prefix);
            if (index < 0)
            {
                return Enumerable.Empty<string>();
            }
            var current = dictionary[index];
            var startWithPrefix = new List<DictionaryEntry>();
            while (current.Word.StartsWith(prefix))
            {
                startWithPrefix.Add(current);
                index++;
                if (index >= dictionary.Length)
                {
                    break;
                }
                current = dictionary[index];
            }
            return startWithPrefix
                .OrderByDescending(e => e.Count)
                .Take(5)
                .Select(e => e.Word);
        }
    }
}