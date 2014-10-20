using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Timers;


namespace SeriesService
{
    class SeriesService : ServiceBase
    {

        private List<Timer> showTimers;

        public SeriesService()
        {
            showTimers = new List<Timer>();

            var s = new Show();

        }

        protected override void OnStart(string[] args)
        {
            // TODO: start service logic
            var shows = SeriesFileLoader.LoadShowsFromFolder(args[0]);

            base.OnStart(args);
        }

        // read rss
        //var show = SyndicationFeed.Load()
    }
}
