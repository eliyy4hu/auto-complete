using Autofac;
using Core.PreProcess;
using Core.Readers;
using Core.Services;
using Core.TextProviders;
using System.Collections.Generic;

namespace Core
{
    public static class StartUp
    {
        public static IContainer InjectDependencies(ProcessOptions options)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CommonReader>()
                .As<TextReaderBase>();

            if (options.Action == Core.Action.Init || options.Action == Core.Action.Update)
            {
                if (options.TargetType == TargetType.File)
                {
                    builder.RegisterType<FileTextProvider>()
                        .As<ITextProvider>()
                        .WithParameter(new TypedParameter(typeof(IEnumerable<string>), new List<string> { options.Target }));
                }
                else
                {
                    builder.RegisterType<DirectoryTextProvider>()
                        .As<ITextProvider>()
                        .WithParameter(new TypedParameter(typeof(string), options.Target));
                }
            }

            builder.RegisterType<Preprocessor>()
                .As<IWordsPreprocessor>();

            builder.RegisterType<DataContext>().AsSelf();

            builder.RegisterType<InMemoryBinSearchDictionaryService>()
                .As<IDictionaryReader>();

            builder.RegisterType<SimpleDictionaryService>()
                .As<IDictionaryUpdater>();

            builder.RegisterType<TextProcessor>().AsSelf();

            var container = builder.Build();
            return container;
        }
    }
}