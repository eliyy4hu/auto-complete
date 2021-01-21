using System;

namespace Core
{
    [Serializable]
    public class GetWordsByPrefixCommand : ICommand<string>
    {
        public GetWordsByPrefixCommand(string prefix)
        {
            Body = prefix;
        }

        public override string Command => "get";
    }
}