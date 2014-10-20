using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SeriesService
{

    /// <summary>
    /// The file structure for each serie contemplates:
    /// <Show>
    ///     <name></name>
    ///     <url></url>
    ///     <frequency></frequency>
    /// </Show>
    /// </summary>
    class SeriesFileLoader
    {
        /// <summary>
        /// Load the series files from a folder and returns an array of shows
        /// </summary>
        /// <param name="folder">Folder Name</param>
        public static List<Show> LoadShowsFromFolder(string folder)
        {
            var shows = new List<Show>();

            folder = Directory.GetCurrentDirectory() + "\\" + folder + "\\";

            // list files in this folder
            foreach (var file in Directory.GetFiles(folder))
            {
                // load each in a show object
                var show = LoadXmlToShow(file);

                if (show != null)
                    shows.Add(show);

            }

            return shows;
        }

        private static Show LoadXmlToShow(string file)
        {
            var show = new Show();

            try
            {
                var doc = XDocument.Load(file);

                // iterate through the XMLDocument
                foreach (var item in doc.Root.Descendants())
                {
                    switch (item.Name.ToString())
                    {
                        case "Name":
                            show.Name = item.Value;
                            break;
                        case "URL":
                            show.URL = item.Value;
                            break;
                        case "frequency":
                            show.Frequency = FrequencyValues.weekly;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return show;

        }

    }
}
