namespace Core
{
    public class ProcessOptions
    {
        public Action Action;
        public TargetType TargetType;
        public string Target;
    }

    public enum Action
    {
        Read,
        Init,
        Update,
        Clear,
    }

    public enum TargetType
    {
        File,
        Directory
    }
}