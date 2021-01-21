using Core.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class DictionaryUpdaterService :
        IDictionaryUpdater, IDictionaryReader
    {
        private const int Min_Word_Occurance_Count = 3;
        private DataContext context;

        public DictionaryUpdaterService(DataContext context)
        {
            this.context = context;
        }

        public void ClearDictionary()
        {
            context.Database.ExecuteSqlRaw($"delete from {nameof(context.FrequencyDictionary)}");
        }

        public IEnumerable<string> GetMostFrequenciesWords(string prefix, int count)
        {
            return context.FrequencyDictionary
                .Where(e => e.Word.StartsWith(prefix))
                .OrderByDescending(e => e.Count)
                .Take(count)
                .Select(e => e.Word);
        }

        public void InitDictionary(IEnumerable<string> words)
        {
            var freqs = GetFrequencyDictionary(words);
            foreach (var word in freqs)
            {
                context.FrequencyDictionary
                    .Add(new DictionaryEntry { Word = word.Key, Count = word.Value });
            }
            context.SaveChanges();
        }

        public bool Initialized()
        {
            return context.FrequencyDictionary.Any();
        }

        public void UpdateDictionary(IEnumerable<string> words)
        {
            var freqs = GetFrequencyDictionary(words);
            foreach (var item in freqs)
            {
                var existed = context.FrequencyDictionary.Find(item.Key);
                if (existed is null)
                {
                    context.FrequencyDictionary
                    .Add(new DictionaryEntry { Word = item.Key, Count = item.Value });
                }
                else
                {
                    existed.Count += freqs[item.Key];
                }
            }
            context.SaveChanges();
        }

        private Dictionary<string, int> GetFrequencyDictionary(IEnumerable<string> words)
        {
            return TextUtils.GetEntriesCount(words)
                .Where(f => f.Value > Min_Word_Occurance_Count)
                .ToDictionary(f => f.Key, f => f.Value);
        }
    }
}