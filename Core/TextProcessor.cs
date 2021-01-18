using Core.PreProcess;
using Core.Services;
using Core.TextProviders;
using Core.Utils;
using System;
using System.Collections.Generic;

namespace Core
{
    public class TextProcessor
    {
        private readonly IWordsPreprocessor wordsPreprocessor;
        private readonly IDictionaryUpdater dictionaryUpdater;
        private readonly ITextProvider textProvider;

        public TextProcessor(
            IWordsPreprocessor wordsPreprocessor,
            IDictionaryUpdater dictionaryUpdater,
            ITextProvider textProvider)
        {
            this.wordsPreprocessor = wordsPreprocessor;
            this.dictionaryUpdater = dictionaryUpdater;
            this.textProvider = textProvider;
        }

        public Result Process(Action action)
        {
            var validationResult = ValidateInput(action);
            if (!validationResult.IsSucceed)
                return validationResult;
            var texts = textProvider.GetTexts();
            ProcessTexts(texts, action);
            return Result.Success();
        }

        private void ProcessTexts(IEnumerable<string> texts, Action action)
        {
            foreach (var text in texts)
            {
                var result = ProcessText(text, action);
                if (!result.IsSucceed)
                {
                    Console.WriteLine(result.Message);
                }
                if (action == Action.Init && result.IsSucceed)
                {
                    action = Action.Update;
                }
            }
        }

        


        private Result ProcessText(string text, Action action)
        {
            var words = TextUtils.SplitTextByWords(text);
            wordsPreprocessor.PreprocessWords(words);
            return ProcessWords(words, action);
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

        private Result ValidateInput(Action action)
        {
            var inited = dictionaryUpdater.Initialized();
            if (action == Action.Init && inited)
            {
                return Result.Error("Dictionary already initilized. Use clear or update commands");
            }
            if (action == Action.Update && !inited)
            {
                return Result.Error("Dictionary isn't initilized yet. Use init command to do it");
            }
            return Result.Success();
        }
    }
}