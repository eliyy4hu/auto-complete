using Autofac;
using CommandLine;
using Core;
using Core.Services;
using Core.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private const int Count = 5;

        private static void Main(string[] args)
        {
            Console.WriteLine("Loading");
            args = args.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var options = Parser.Default.ParseArguments<ConsoleOptions>(args);
            options.WithParsed(x => WithParsed(x));
        }

        private static void WithParsed(ConsoleOptions consoleOptions)
        {
            var validateArgs = ValidateArguments(consoleOptions);
            if (!validateArgs.IsSucceed)
            {
                logger.Error(validateArgs);
                return;
            }

            var options = ConvertToProcessOptions(consoleOptions);
            var result = Start(options);
            if (result.IsSucceed)
            {
                logger.Info(result);
            }
            else
            {
                logger.Error(result);
            }
        }

        private static Result Start(ProcessOptions options)
        {
            IContainer container = InjectDependencies(options);

            using var scope = container.BeginLifetimeScope();
            try
            {
                return ResolveScope(scope, options);
            }
            catch (Exception e)
            {
                logger.Debug(e);
                return Result.Error("Unepected error. View log file to see a stacktrace");
            }
        }

        public static IContainer InjectDependencies(ProcessOptions options)
        {
            var builder = new ContainerBuilder();
            builder = DI.InjectUpdating(options, builder);
            builder = DI.InjectReading(builder);
            var container = builder.Build();
            return container;
        }

        private static Result ResolveScope(ILifetimeScope scope, ProcessOptions options)
        {
            if (options.Action == Core.Action.Clear)
            {
                scope.Resolve<IDictionaryUpdater>().ClearDictionary();
                return Result.Success("Clear succeed");
            }
            else if (options.Action == Core.Action.Init || options.Action == Core.Action.Update)
            {
                return scope.Resolve<TextProcessor>().Process(options.Action);
            }
            else if (options.Action == Core.Action.Read)
            {
                ProcessInput(scope.Resolve<IDictionaryReader>());
            }
            return Result.Success();
        }

        private static void ProcessInput(IDictionaryReader dictionaryReader)
        {
            var input = Input.ReadLineWithCancel();
            while (!string.IsNullOrWhiteSpace(input))
            {
                logger.Debug(input);
                var result = dictionaryReader
                    .GetMostFrequenciesWords(input, Count).ToList();
                result.ForEach(w => Console.WriteLine(w));
                Console.WriteLine();
                input = Input.ReadLineWithCancel();
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

            if (!string.IsNullOrWhiteSpace(options.InputDirectory))
            {
                result.TargetType = TargetType.Directory;
                result.Target = options.InputDirectory;
            }
            else if (!string.IsNullOrWhiteSpace(options.InputFile))
            {
                result.TargetType = TargetType.File;
                result.Target = options.InputFile;
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