using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Core.Services
{
    public class InMemoryBinSearchDictionaryReader : IDictionaryReader
    {
        private DataContext context;
        private DictionaryEntry[] dictionary;
        private DateTime lastUpdate;
        private readonly object lockObj = new object();

        public InMemoryBinSearchDictionaryReader(DataContext context)
        {
            this.context = context;
            dictionary = context.FrequencyDictionary.ToArray();
        }

        public IEnumerable<string> GetMostFrequenciesWords(string prefix, int count)
        {
            new Thread(CheckForUpdates).Start();
            lock (lockObj)
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

        private void CheckForUpdates()
        {
            if (State.LastUpdate > lastUpdate)
            {
                Console.WriteLine("Found new update. Loading data");
                lastUpdate = State.LastUpdate;
                var newDictionary = context.FrequencyDictionary.ToArray();
                lock (lockObj)
                {
                    dictionary = newDictionary;
                }
            }
            context.SaveChanges();
        }
    }
}