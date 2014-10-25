using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesService
{
    class ServiceRunner
    {
        static void Main()
        {
            // test file loading
            var s = SeriesFileLoader.LoadShowsFromFolder("Series");

            // Download new episodes
            s.ForEach((e) => e.CheckForNewEpisode());


            // Update files
            s.ForEach(e => e.SaveToXML());

            Console.WriteLine(":P");

        }
    }
}
