using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2HTML
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] != "--target")
            {
                Console.WriteLine("use --target filename");
                return;
            }

            CharWithColor[,] bufferContent = ConsoleBufferReader.ReadConsoleBuffer();
            var generateHtml = new HtmlGenerator()
                .GenerateHtmlFromArray(bufferContent);

            string path = args[1];
            File.WriteAllText(path, generateHtml);
        }
    }
}
