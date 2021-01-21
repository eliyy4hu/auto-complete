using Autofac;
using Core.PreProcess;
using Core.Readers;
using Core.Services;
using Core.TextProviders;
using System.Collections.Generic;

namespace Core
{
    public static class DI
    {
        public static ContainerBuilder Inject(ContainerBuilder builder, ProcessOptions options)
        {
            builder = InjectReading(builder);
            builder = InjectUpdating(options,builder);
            return builder;
        }
        public static ContainerBuilder InjectReading(ContainerBuilder builder)
        {
            builder = InjectCommon(builder);
            builder.RegisterType<InMemoryBinSearchDictionaryReader>()
                            .As<IDictionaryReader>();
            return builder;
        }

        public static ContainerBuilder InjectCommon(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().AsSelf();
            return builder;
        }

        public static ContainerBuilder InjectUpdating(ProcessOptions options, ContainerBuilder builder)
        {
            builder = InjectCommon(builder);
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


            builder.RegisterType<DictionaryUpdaterService>()
                .As<IDictionaryUpdater>();

            builder.RegisterType<TextProcessor>().AsSelf();
            return builder;
        }
    }
}