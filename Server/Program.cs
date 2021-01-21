using Autofac;
using CommandLine;
using Core;
using Core.Services;
using Core.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    internal class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<ServerConsoleOptions>(args);
            options.WithParsed(x => StartWithOptions(x));
        }

        private static void StartWithOptions(ServerConsoleOptions x)
        {
            if (x.Port.HasValue)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<Server>().AsSelf()
                    .WithParameter(new TypedParameter(typeof(int),  x.Port.Value));
                var readInject = DI.InjectReading(builder);
                var server = readInject.Build().Resolve<Server>();
                ProcessInput();
            }
            else
            {
                Console.WriteLine("Enter port to start");
                Environment.Exit(-1);
            }
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
            var result = Process(options);
            if (result.IsSucceed)
            {
                logger.Info(result);
            }
            else
            {
                logger.Error(result);
            }
        }

        private static Result Process(ProcessOptions options)
        {
            IContainer container = DI.InjectUpdating(options, new ContainerBuilder()).Build();

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
            return Result.Error("unresolved input");
        }

        private static void ProcessInput()
        {
            var input = Input.ReadLineWithCancel();

            while (true)
            {
                logger.Debug(input);
                var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var options = Parser.Default.ParseArguments<ConsoleOptions>(args);
                options.WithParsed(x => WithParsed(x));
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
            else if (options.Clear)
            {
                return Core.Action.Clear;
            }
            else
            {
                return Core.Action.Update;
            }
        }
    }
}