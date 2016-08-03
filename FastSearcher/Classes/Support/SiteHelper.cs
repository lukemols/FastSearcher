using System;
using System.Collections.Generic;
using System.Text;

namespace FastSearcher.Classes.Support
{
    class SiteHelper
    {
        static SiteHelper instance;
        public static SiteHelper Instance { get { if (instance == null) instance = new SiteHelper(); return instance; } }

        private SiteHelper() { }

        public async void GoToPublisherWebsite()
        {
            string lang = LanguageHelper.Instance.GetActualLanguage();
            string url = "https://lmsoftit.wordpress.com/";

            if (lang.Contains("en"))
            {
                url = "https://lmesoften.wordpress.com/";
            }
            else if (lang.Contains("es"))
            {
                url = "https://lmesoftes.wordpress.com/";
            }

            Uri uri = new Uri(url);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
