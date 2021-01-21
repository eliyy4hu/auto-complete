using CommandLine;
using Core.Utils;
using System;
using System.Linq;
using System.Net;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<ClientConsoleOptions>(args);
            options.WithParsed(x => WithParsed(x));
        }

        private static void WithParsed(ClientConsoleOptions x)
        {
            if (string.IsNullOrEmpty(x.Host) && string.IsNullOrEmpty(x.Ip))
            {
                Console.WriteLine("Enter host name or IP");
                Environment.Exit(0);
            }
            if (!x.Port.HasValue)
            {
                Console.WriteLine("Enter port");
                Environment.Exit(0);
            }
            if (!string.IsNullOrEmpty(x.Host))
            {
                ProcessInput(x.Host, x.Port.Value);
            }
            else
            {
                IPAddress ip = null;
                try
                {
                    ip = IPAddress.Parse(x.Ip);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid ip");
                }
                ProcessInput( x.Ip, x.Port.Value);
            }
        }

        private static void ProcessInput( string host, int port)
        {
            var input = Input.ReadLineWithCancel();

            while (!string.IsNullOrWhiteSpace(input))
            {
                using var client = new Client(host, port);
                var result = client.SendGetByPrefix(input);
                if (result != null)
                {
                    result.ToList().ForEach(x => Console.WriteLine(x));
                    Console.WriteLine();
                }
                input = Input.ReadLineWithCancel();
            }
        }
    }
}