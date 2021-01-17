using Core.PreProcess;
using Core.Readers;
using Core.Services;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    public class TextFileProcessor
    {
        private const int Words_Count = 5;

        private readonly TextReaderBase textReader;
        private readonly IWordsPreprocessor wordsPreprocessor;
        private readonly IDictionaryUpdater dictionaryUpdater;
        private readonly IDictionaryReader dictionaryReader;

        public TextFileProcessor(
            TextReaderBase textReader,
            IWordsPreprocessor wordsPreprocessor,
            IDictionaryUpdater dictionaryUpdater,
            IDictionaryReader dictionaryReader)
        {
            this.textReader = textReader;
            this.wordsPreprocessor = wordsPreprocessor;
            this.dictionaryUpdater = dictionaryUpdater;
            this.dictionaryReader = dictionaryReader;
        }

        public Result Process(ProcessOptions options)
        {
            var validationResult = ValidateInput(options);
            if (!validationResult.IsSucceed)
                return validationResult;

            return ProcessOptions(options);
        }

        private Result ProcessOptions(ProcessOptions options)
        {
            if (options.Action == Action.Clear)
            {
                dictionaryUpdater.ClearDictionary();
                return Result.Success("Clear successfull");
            }
            else if (options.Action == Action.Init || options.Action == Action.Update)
            {
                if (options.TargetType == TargetType.Directory)
                {
                    ProcessDirectory(options.Target, options.Action);
                }
                else if (options.TargetType == TargetType.File)
                {
                    return ProcessTextFile(options.Target, options.Action);
                }
            }
            else if (options.Action == Action.Read)
            {
                ProcessInput();
            }
            return Result.Success();
        }

        private void ProcessInput()
        {
            var input = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(input))
            {
                var result = dictionaryReader
                    .GetMostFrequenciesWords(input, Words_Count).ToList();
                result.ForEach(w => Console.WriteLine(w));
                Console.WriteLine();
                input = Console.ReadLine();
            }
        }

        private void ProcessDirectory(string target, Action action)
        {
            var files = GetFilesRecursive(target);
            files = files.Where(f => textReader.IsSupportedFileExtension(f)).ToList();
            Console.WriteLine($"Total number of supported files: {files.Count}");
            //files = GetEveryN(files, 20);
            foreach (var filename in files)
            {
                if (textReader.IsSupportedFileExtension(filename))
                {
                    var result = ProcessTextFile(filename, action);
                    if (!result.IsSucceed)
                    {
                        Console.WriteLine(result.Message);
                    }
                    if (action == Action.Init && result.IsSucceed)
                    {
                        action = Action.Update;
                    }
                }
                else
                {
                    Console.WriteLine($"{filename} - skip (unsupported)");
                }
            }
        }

        private List<string> GetEveryN(List<string> files, int n)
        {
            var result = new List<string>();
            for (var i = 0; i < files.Count; i += n)
            {
                result.Add(files[i]);
            }
            return result;
        }

        private List<string> GetFilesRecursive(string sDir)
        {
            var result = new List<string>();
            try
            {
                foreach (var d in Directory.GetDirectories(sDir))
                {
                    foreach (var f in Directory.GetFiles(d))
                    {
                        result.Add(f);
                    }
                    result.AddRange(GetFilesRecursive(d));
                }
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return result;
        }

        private Result ProcessTextFile(string file, Action action)
        {
            Console.WriteLine($"{file} - processing...");
            if (textReader.IsSupportedFileExtension(file))
            {
                var text = textReader.ReadText(file);
                var words = TextUtils.SplitTextByWords(text);
                wordsPreprocessor.PreprocessWords(words);
                return ProcessWords(words, action);
            }
            else
            {
                return Result.Error($"Unsupported format.Supported formats are{string.Join(", ", textReader.SupportedExtensions)}");
            }
        }

        private Result ProcessWords(List<string> words, Action action)
        {
            try
            {
                switch (action)
                {
                    case Action.Update:
                        dictionaryUpdater.UpdateDictionary(words);
                        return Result.Success("Update successfull");

                    case Action.Init:
                        dictionaryUpdater.InitDictionary(words);
                        return Result.Success("Init successfull");

                    default:
                        throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                return Result.Error(e.ToString());
            }
        }

        private Result ValidateInput(ProcessOptions options)
        {
            var inited = dictionaryUpdater.Initialized();
            if (options.Action == Action.Init && inited)
            {
                return Result.Error("Dictionary already initilized. Use clear or update commands");
            }
            if (options.Action == Action.Update && !inited)
            {
                return Result.Error("Dictionary isn't initilized yet. Use init command to do it");
            }
            return Result.Success();
        }
    }
}