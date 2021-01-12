using Core;
using Core.Services;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DataContext())
            {
                var dictionaryService = new SimpleDictionaryService(context);
                dictionaryService.InitDictionary(new List<string> { "abc", "abcd", "abc" });
                dictionaryService.UpdateDictionary(new List<string> {  "abcd", "ef" });
                dictionaryService.ClearDictionary();
            }
        }
    }
}
