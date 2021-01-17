using CommandLine;
using Core;
using Core.PreProcess;
using Core.Readers;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            args = args.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var options = Parser.Default.ParseArguments<ConsoleOptions>(args);
            options.WithParsed(x => Console.WriteLine(WithParsed(x)));
        }

        private static Result WithParsed(ConsoleOptions options)
        {
            var validateArgs = ValidateArguments(options);
            if (!validateArgs.IsSucceed)
            {
                return validateArgs;
            }

            using (var context = new DataContext())
            {
                var reader = new CommonReader();
                var preprocessor = new Preprocessor();
                var dictionaryReader = new InMemoryBinSearchDictionaryService(context);
                var dictionaryUpdater = new InMemoryBinSearchDictionaryService(context);
                var processor = new TextFileProcessor(reader, preprocessor, dictionaryUpdater, dictionaryReader);
                return processor.Process(ConvertToProcessOptions(options));
            }
        }

        private static Result ValidateArguments(ConsoleOptions options)
        {
            var commandsActionValues = new List<bool>
                { options.Init,
                options.Update,
                options.Clear };
            if (commandsActionValues.Count(x => x) > 1)
            {
                return Result.Error("Arguments should contain only one action argument(Init, Update, Clear)");
            }
            if (options.Init || options.Update)
            {
                var targetArguments = new string[] { options.InputFile, options.InputDirectory };
                if (targetArguments.All(x => string.IsNullOrEmpty(x)))
                {
                    return Result.Error("Missing required target argument. To update dictionary specify file or directory");
                }
                if (targetArguments.Count(x => !string.IsNullOrEmpty(x)) > 1)
                {
                    return Result.Error("Only one target argument allowed with update or init action arguments");
                }
            }
            return Result.Success();
        }

        private static ProcessOptions ConvertToProcessOptions(ConsoleOptions options)
        {
            var result = new ProcessOptions
            {
                Action = GetAction(options)
            };

            if (string.IsNullOrWhiteSpace(options.InputDirectory))
            {
                result.TargetType = TargetType.Directory;
            }
            if (string.IsNullOrWhiteSpace(options.InputFile))
            {
                result.TargetType = TargetType.File;
            }
            return result;
        }

        private static Core.Action GetAction(ConsoleOptions options)
        {
            if (options.Init)
            {
                return Core.Action.Init;
            }
            else if (options.Update)
            {
                return Core.Action.Update;
            }
            else if (options.Clear)
            {
                return Core.Action.Clear;
            }
            else 
            {
                return Core.Action.Read;
            }
        }
    }
}