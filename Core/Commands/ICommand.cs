using System;

namespace Core
{
    [Serializable]
    public abstract class ICommand<T>
    {
        public abstract string Command { get; }
        public T Body { get; set; }
    }
}