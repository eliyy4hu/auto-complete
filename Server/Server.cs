using Core;
using Core.Services;
using Core.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    internal class Server
    {
        private const int Buffer_Size = 4096;
        private readonly TcpListener server = null;
        private readonly IDictionaryReader dictionaryReader;

        public Server(int port, IDictionaryReader reader)
        {
            server = new TcpListener(IPAddress.Loopback, port);
            server.Start();
            this.dictionaryReader = reader;
            new Thread(StartListener).Start();
        }

        private void StartListener()
        {
            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }

        public void HandleDeivce(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            byte[] bytes = new byte[Buffer_Size];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    Byte[] reply = GetReply(bytes);
                    stream.Write(reply, 0, reply.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }

        private byte[] GetReply(byte[] bytes)
        {
            var command = Serialize.ByteArrayToObject(bytes);
            if (!(command is GetWordsByPrefixCommand getWordsByPrefix))
            {
                return new byte[0];
            }

            var prefix = getWordsByPrefix.Body;
            var result = dictionaryReader.GetMostFrequenciesWords(prefix, 5).ToArray();
            return Serialize.ObjectToByteArray(result);
        }
    }
}