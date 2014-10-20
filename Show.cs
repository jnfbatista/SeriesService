using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// READ FEEDS
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml;

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
            var reader = XmlReader.Create(URL);
            var feed = SyndicationFeed.Load(reader);

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
