using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Utils
{
    public static class Serialize
    {
        public static object ByteArrayToObject(byte[] arrBytes)
        {
            if (arrBytes.Length == 0)
            {
                return null;
            }
            using var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
}