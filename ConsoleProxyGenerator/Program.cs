using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.ProxyGeneration;

namespace ConsoleProxyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            ProxyExtractor extractor = new ProxyExtractor();
            if (File.Exists(args[0]))
            {
                Assembly asm = Assembly.LoadFile(args[0]);
                var extracted = extractor.ExtractAssembly(asm);

                DefaultFormatter formatter = new DefaultFormatter();
                var contentList = formatter.FormatProxyExtraction(extracted);

                foreach (var content in contentList)
                {
                    Console.WriteLine(content);
                }
                
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
            }
        }
    }
}
