using Core.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class SimpleDictionaryService : IDictionaryUpdaterService
    {
        private DataContext context;

        public SimpleDictionaryService(DataContext context)
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
                .Select(e=>e.Word);
        }

        public void InitDictionary(IEnumerable<string> words)
        {
            var freqs = TextUtils.GetEntriesCount(words);
            foreach (var word in freqs)
            {
                context.FrequencyDictionary
                    .Add(new DictionaryEntry { Word = word.Key, Count = word.Value });
            }
            context.SaveChanges();
        }

        public void SaveResult()
        {
            
        }

        public void UpdateDictionary(IEnumerable<string> words)
        {
            var freqs = TextUtils.GetEntriesCount(words);
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
    }
}
