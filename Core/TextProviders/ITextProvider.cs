using System.Collections.Generic;

namespace Core.TextProviders
{
    public interface ITextProvider
    {
        public IEnumerable<string> GetTexts();
    }
}