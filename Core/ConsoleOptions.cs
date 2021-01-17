using CommandLine;

namespace Core
{
    public class ConsoleOptions
    {
        [Option('i', "init", Required = false,
           HelpText = "Init dictionary")]
        public bool Init { get; set; }

        [Option('u', "update", Required = false,
            HelpText = "Update dictionary")]
        public bool Update { get; set; }

        [Option('c', "clear",
          HelpText = "Prints all messages to standard output.")]
        public bool Clear { get; set; }

        [Option('f', "file", Required = false,
            HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('d', "directory", Required = false,
            HelpText = "Input file to be processed.")]
        public string InputDirectory { get; set; }
    }
}