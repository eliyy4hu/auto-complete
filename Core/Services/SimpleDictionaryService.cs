using Core.Models;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            context.Database.ExecuteSqlRaw($"delete from {nameof(context.DictionaryEntries)}");
        }

        public void InitDictionary(IEnumerable<string> words)
        {
            var freqs = new FrequencyAnalyser().GetEntriesCount(words);
            foreach (var word in freqs)
            {
                context.DictionaryEntries
                    .Add(new DictionaryEntry { Word = word.Key, Count = word.Value });
            }
            context.SaveChanges();
        }

        public void UpdateDictionary(IEnumerable<string> words)
        {
            var freqs = new FrequencyAnalyser().GetEntriesCount(words);
            foreach (var item in freqs)
            {
                var existed = context.DictionaryEntries.Find(item.Key);
                if (existed is null)
                {
                    context.DictionaryEntries
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
