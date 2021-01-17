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
            var options = Parser.Default.ParseArguments<Options>(args);
            options.WithParsed(x => Console.WriteLine(WithParsed(x)));
        }

        private static Result WithParsed(Options options)
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
                var dictionaryService = new InMemoryBinSearchDictionaryService(context);
                var processor = new TextFileProcessor(reader, preprocessor, dictionaryService, dictionaryService);
                return processor.Process(ConvertToProcessOptions(options));
            }
        }

        private static Result ValidateArguments(Options options)
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

        private static ProcessOptions ConvertToProcessOptions(Options options)
        {
            var result = new ProcessOptions();
            if (options.Clear)
            {
                result.Action = Core.Action.Clear;
                return result;
            }
            else if (options.Init)
            {
                result.Action = Core.Action.Init;
            }
            else if (options.Update)
            {
                result.Action = Core.Action.Update;
            }
            else if (!options.Clear && !options.Init && !options.Update)
            {
                result.Action = Core.Action.Read;
            }

            if (string.IsNullOrWhiteSpace(options.InputDirectory))
            {
                result.TargetType = TargetType.Directory;
            }
            else if (string.IsNullOrWhiteSpace(options.InputFile))
            {
                result.TargetType = TargetType.File;
            }
            return result;
        }
    }
}