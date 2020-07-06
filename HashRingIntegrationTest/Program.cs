using ConsistentHashRing;
using System;

namespace HashRingIntegrationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consistent Hash Ring Test");

            // Make location keys for the ring
            string[] locationKeys = {"A", "B", "C", "D" };

            // Instantiate the HashRing
            HashRing<string> hashRing = new HashRing<string>();
            hashRing.RingLocationReceived += new HashRing<string>.RingLocationReceivedItemDelegate((key, item) =>
            {
                Console.WriteLine($"Received on {key}: {item}");
            });

            // Add the location keys
            for(var i = 0; i < locationKeys.Length; i++)
            {
                var location = hashRing.AddLocation(locationKeys[i]);
            }

            Console.WriteLine($"Locations in the HashRing {hashRing.LocationCount}");
            // Now add items to the Ring and display the location counts
            while(true)
            {
                Console.WriteLine("Enter some text (empty line quits)");
                var someText = Console.ReadLine();
                if (someText == string.Empty)
                    break;

                hashRing.AddItem(someText);

                Console.WriteLine();

            }

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}
