using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogFileReader
{
   class Program
    {
        public static void Main(string[] args)
        {
            var fileReader = new LogFileParser(AppContext.BaseDirectory, @"logs\programming-task-example-data.log");

            fileReader.Parse();

            var top3ips = fileReader.GetTopThreeActiveIPs();

            foreach (var item in top3ips)
            {
                Console.WriteLine(item);
            }

            var top3urls = fileReader.GetTopThreeVisitedURLs();

            foreach (var item in top3urls)
            {
                Console.WriteLine(item);
            }

        }
    }
}
