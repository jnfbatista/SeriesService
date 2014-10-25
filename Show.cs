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

        public string FileName { get; set; }

        public FrequencyValues Frequency = FrequencyValues.weekly;

        public Show()
        {
            SavePath = Directory.GetCurrentDirectory() + "\\Downloads\\";
        }


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

            var downloadAll = LastDownload == DateTime.MinValue;
            // Read the rss

            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            // fetch feed as string
            var content = webClient.OpenRead(URL);
            if (content == null) return;

            var contentReader = new StreamReader(content);
            var rssFeedAsString = contentReader.ReadToEnd();
            // convert feed to XML using LINQ to XML and finally create new XmlReader object
            var feed = SyndicationFeed.Load(XDocument.Parse(rssFeedAsString).CreateReader());

            if (feed == null) return;
            List<SyndicationItem> downloadList;

            // Reduce the set of downloads only to the desired
            if (downloadAll)
            {
                downloadList = feed.Items.ToList();
            }
            else
            {
                var dList = from item in feed.Items
                            where item.PublishDate < new DateTimeOffset(LastDownload)
                            select item;

                downloadList = dList.ToList();
            }

            if (downloadList.Count > 0)
            {
                if (!Directory.Exists(SavePath))
                    Directory.CreateDirectory(SavePath);
            }

            // Download all that apply
            foreach (var item in downloadList)
            {
                var links = from link in item.Links
                    select link.Uri.AbsoluteUri;

                if (!links.Any()) return;

                var name = item.Id.Substring(0, item.Id.Length - 1).Split('/').Last() + ".torrent";
                Console.WriteLine(name);

                DownloadFile(links.ToArray(), SavePath + name);

                if (LastDownload < item.PublishDate.DateTime)
                    LastDownload = item.PublishDate.DateTime;
            }
            
            //Console.WriteLine("");
        }

        private void DownloadFile(string[] links, string filename)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            foreach (var url in links)
            {
                try
                {
                    webClient.DownloadFile(new Uri(url), filename);

                    return;
                }
                catch (WebException webException)
                {
                    Console.WriteLine("Message: " + webException.Message + "\nStatus:" + webException.Status);
                }
            }
            
        }


        #region File IO
        public static Show LoadXml(string file)
        {
            var show = new Show()
            {
                FileName = file
            };

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
                        case "LastDownload":
                            show.LastDownload = DateTime.Parse(item.Value);
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

        public void SaveToXML()
        {
            var file = new XDocument(
                new XElement("Show",
                    new XElement("Name", Name),
                    new XElement("URL", URL),
                    new XElement("LastDownload", LastDownload)
                    ));


            file.Save(FileName);

        }

        #endregion

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
