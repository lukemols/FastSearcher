using System;
using System.Collections.Generic;
using System.Text;

namespace FastSearcher.Classes.Support
{
    class TwitterHelper
    {
        static TwitterHelper instance;
        public static TwitterHelper Instance { get { if (instance == null) instance = new TwitterHelper(); return instance; } }

        private TwitterHelper() { }

        public async void GoToTwitterProfile()
        {
            var options = new Windows.System.LauncherOptions();
            options.FallbackUri = new Uri("http://www.twitter.com/LMESoft");
            await Windows.System.Launcher.LaunchUriAsync(new Uri("twitter:profile?username=LMESoft"), options);
        }
    }
}
