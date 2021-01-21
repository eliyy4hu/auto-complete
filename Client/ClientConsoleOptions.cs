using CommandLine;

namespace Client
{
    public class ClientConsoleOptions
    {
        [Option('i', "ip", Required = false,
           HelpText = "IP")]
        public string Ip { get; set; }

        [Option("host", Required = false,
           HelpText = "Host name")]
        public string Host { get; set; }

        [Option('p', "port", Required = false,
            HelpText = "Port")]
        public int? Port { get; set; }
    }
}