using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public static class Input
    {
        public  static string ReadLineWithCancel()
        {
            string result = null;
            StringBuilder buffer = new StringBuilder();
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                Console.Write(info.KeyChar);
                buffer.Append(info.KeyChar);
                info = Console.ReadKey(true);
            }

            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }
            Console.WriteLine();
            return result;
        }
    }
}
