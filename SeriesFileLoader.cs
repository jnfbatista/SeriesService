using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

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

            var fileReader = XmlReader.Create(file);

            while (fileReader.Read())
            {
                if (fileReader.NodeType != XmlNodeType.Element) continue;

                switch (fileReader.Name)
                {
                    case "Name":
                        fileReader.Read();
                        show.Name = fileReader.Value;
                        break;
                    case "URL":
                        fileReader.Read();
                        show.URL = fileReader.Value;
                        break;
                    case "frequency":
                        fileReader.Read();
                        show.Frequency = FrequencyValues.weekly;
                        break;
                    default:
                        break;
                }

                //}
            }

            return show;

        }

    }
}
