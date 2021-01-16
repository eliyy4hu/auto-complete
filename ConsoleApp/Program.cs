using CommandLine;
using Core;
using Core.Readers;
using Core.Services;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private const int WordsCount = 5;

        private static void Main(string[] args)
        {
            args = args.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var options = Parser.Default.ParseArguments<Options>(args);

            options.WithParsed(x => WithParsed(x));
        }

        public static Result WithParsed(Options options)
        {
            var dictionaryService = new SimpleDictionaryService(new DataContext());
            using (var context = new DataContext())
            {
                var reader = new CommonReader();

                var validationResult = ValidateInput(options);
                if (!validationResult.IsSucceed)
                    return validationResult;

                return ProcessOptions(options, dictionaryService, reader);
            }
        }

        private static Result ProcessOptions(Options options, SimpleDictionaryService dictionaryService, TextReaderBase reader)
        {
            if (options.Clear)
            {
                dictionaryService.ClearDictionary();
                return Result.Success("Clear successfull");
            }
            if (!string.IsNullOrEmpty(options.InputFile))
            {
                return ProcessTextFile(options.InputFile, options, dictionaryService, reader);
            }
            else if (!string.IsNullOrEmpty(options.InputDirectory))
            {
            }
            else
            {
                var input = Console.ReadLine();
                while (!string.IsNullOrWhiteSpace(input))
                {
                    var result = dictionaryService
                        .GetMostFrequenciesWords(input, WordsCount).ToList();
                    result.ForEach(w => Console.WriteLine(w));
                    Console.WriteLine();
                    input = Console.ReadLine();
                }
            }
            return Result.Success();
        }

        private static Result ProcessTextFile(string file, Options options, SimpleDictionaryService dictionaryService, TextReaderBase reader)
        {
            if (reader.IsSupportedFileExtension(file))
            {
                var text = reader.ReadText(file);
                var words = TextUtils.SplitTextByWords(text);
                return ProcessWords(options, dictionaryService, words);
            }
            else
            {
                return Result.Error($"Unsupported format.Supported formats are{string.Join(", ", reader.SupportedExtensions)}");
            }
        }

        private static Result ProcessWords(Options options, SimpleDictionaryService dictionaryService, List<string> words)
        {
            if (options.Init)
            {
                dictionaryService.InitDictionary(words);
                return Result.Success("Init successfull");
            }
            else if (options.Update)
            {
                dictionaryService.UpdateDictionary(words);
                return Result.Success("Update successfull");
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private static Result ValidateInput(Options options)
        {
            return Result.Success();
        }
    }
}