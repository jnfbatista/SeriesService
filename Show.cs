using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// READ FEEDS
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

namespace SeriesService
{
    class Show
    {

        #region Attributes

        public FrequencyValues Frequency = FrequencyValues.weekly;

        
        public string Name
        {
            get;
            set;
        }

        public DateTime LastDownload
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set; // TODO see if it's a valid url
        }

        public string SavePath
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// checks for a new episode
        /// </summary>
        public void CheckForNewEpisode()
        {
            // Read the rss

            var webClient = new WebClient();
            // hide ;-)
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            // fetch feed as string
            var content = webClient.OpenRead(URL);
            var contentReader = new StreamReader(content);
            var rssFeedAsString = contentReader.ReadToEnd();
            // convert feed to XML using LINQ to XML and finally create new XmlReader object
            var feed = SyndicationFeed.Load(XDocument.Parse(rssFeedAsString).CreateReader());

            // Check for new files
            foreach (var item in feed.Items)
            {
                Console.WriteLine(item.Content);
            }

            // Download all that apply
            Console.WriteLine("");
        }

    }

    /// <summary>
    /// Possible frequency values for checking new episodes
    /// </summary>
    public enum FrequencyValues
    {
        weekly,
        monthly,
        random
    }
}
