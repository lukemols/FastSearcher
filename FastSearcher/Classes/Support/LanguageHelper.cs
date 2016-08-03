using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Windows.Storage;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Threading;

namespace FastSearcher.Classes.Support
{
    class LanguageHelper
    {
        static LanguageHelper instance;
        public static LanguageHelper Instance { get { if (instance == null) instance = new LanguageHelper(); return instance; } }

        private LanguageHelper() { }

        const string LangSetting = "Lang";
        List<string> Languages = new List<string>() { "it", "en", "es" };

        public string GetActualLanguage()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public string GetUserLanguage()
        {
            try
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(LangSetting))
                {
                    string lang = IsolatedStorageSettings.ApplicationSettings[LangSetting] as string;
                    if (Languages.Contains(lang))
                        return lang;
                    else
                        return "";
                }
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        public void SetUserLanguage(string lang)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
           
            if (!settings.Contains(LangSetting))
            {
                settings.Add(LangSetting, lang);
            }
            else
            {
                settings[LangSetting] = lang;
            }
            settings.Save();
        }
    }
}
