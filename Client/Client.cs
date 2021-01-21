using Core;
using Core.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class Client : IDisposable
    {
        private readonly TcpClient client;

        public Client(string server, int port)
        {
            this.client = new TcpClient(server, port);
        }


        public void Dispose()
        {
            client.Close();
        }

        public string[] SendGetByPrefix(string prefix)
        {
            var command = new GetWordsByPrefixCommand(prefix);
            var bytes = Serialize.ObjectToByteArray(command);
            var response = Send(bytes);
            var responseObject = Serialize.ByteArrayToObject(response);
            if (responseObject is string[] strings)
            {
                return strings;
            }
            return null;
        }

        private byte[] Send(byte[] data)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                
                var bytes = stream.Read(data, 0, data.Length);
                data = data.Take(bytes).ToArray();
                stream.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return data;
        }
    }
}