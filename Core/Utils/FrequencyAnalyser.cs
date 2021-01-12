using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public class FrequencyAnalyser
    {
        public Dictionary<string,int> GetEntriesCount(IEnumerable<string> source)
        {
            var result = new Dictionary<string, int>();
            foreach (var item in source)
            {
                if (result.ContainsKey(item))
                {
                    result[item]++;
                }
                else
                {
                    result[item] = 1;
                }
            }
            return result;
        }
    }
}
