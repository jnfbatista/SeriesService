using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesService
{
    class Show
    {

        #region attributes

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
