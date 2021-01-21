using CommandLine;

namespace Server
{
    public class ServerConsoleOptions
    {
        [Option('p', "port", Required = false,
            HelpText = "Port")]
        public int? Port { get; set; }
    }
}