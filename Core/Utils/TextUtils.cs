using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Utils
{
    public static class TextUtils
    {
        public static Dictionary<string, int> GetEntriesCount(IEnumerable<string> source)
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

        public static List<string> SplitTextByWords(string text)
        {
            var regex = new Regex("\\W?(\\w+)\\W");
            var matches = regex.Matches(text);
            var res = new List<string>();
            foreach (Match match in matches)
            {
                res.Add(match.Groups[1].Value);
            }

            return res;
        }
    }
}
