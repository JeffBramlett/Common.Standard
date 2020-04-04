using System;
using Common.Standard.Generic;
using System.Collections.Generic;

namespace ConsolePoolIntoDb
{
    class Program
    {
        private static GenericObjectPool<DbLoader> Loaders ;

        static void Main(string[] args)
        {
            var connectionString = "Data Source=DESKTOP-QMR3K93;Initial Catalog=Testing;Integrated Security=True";
            Loaders = new GenericObjectPool<DbLoader>(100, BalancingMethods.Random);

            int rowCnt = 5000;

            int loaderCnt = 0;

            DateTime start = DateTime.Now;

            for(var row = 0; row < rowCnt; row++)
            {
                List<string> columns = new List<string>();
                for(var col = 0; col < 25; col++)
                {
                    columns.Add($"somevalue{col}");
                }

                var loader = Loaders.AcquireItem(connectionString).Result;
                if(string.IsNullOrEmpty(loader.Name))
                {
                    loaderCnt++;
                    loader.Name = $"Loader {loaderCnt}";
                }
                loader.MaxCount = 500;
                loader.Activate(connectionString);

                loader.LoadIntoDatabase(columns);
            }
            var end = DateTime.Now;
            Console.WriteLine($"{Loaders.Count} loaders started in {end - start}");

            Console.WriteLine("\nPress any key to quit");
            Console.ReadKey();


            Loaders.Dispose();
        }
    }
}
