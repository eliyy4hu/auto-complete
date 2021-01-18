using Core.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class InMemoryBinSearchDictionaryService : IDictionaryUpdater, IDictionaryReader
    {
        private DataContext context;
        private DictionaryEntry[] dictionary;

        public InMemoryBinSearchDictionaryService(DataContext context)
        {
            this.context = context;
            dictionary = context.FrequencyDictionary.ToArray();
        }

        public void ClearDictionary()
        {
            context.Database.ExecuteSqlRaw($"delete from {nameof(context.FrequencyDictionary)}");
        }

        public IEnumerable<string> GetMostFrequenciesWords(string prefix, int count)
        {
            var index = Search.IndexOfFirstGreatOrEqual(dictionary, e => e.Word, prefix);
            var current = dictionary[index];
            var startWithPrefix = new List<DictionaryEntry>();
            while (current.Word.StartsWith(prefix))
            {
                startWithPrefix.Add(current);
                index++;
                current = dictionary[index];
            }
            return startWithPrefix
                .OrderByDescending(e => e.Count)
                .Take(5)
                .Select(e => e.Word);
        }

        public void InitDictionary(IEnumerable<string> words)
        {
            throw new NotImplementedException();
        }

        public bool Initialized()
        {
            return dictionary.Any();
        }

        public void SaveResult()
        {
            throw new NotImplementedException();
        }

        public void UpdateDictionary(IEnumerable<string> words)
        {
            throw new NotImplementedException();
        }
    }
}