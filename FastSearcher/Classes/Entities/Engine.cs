using System;
using System.Collections.Generic;
using System.Text;

namespace FastSearcher.Classes.Entities
{
    class Engine
    {
        public enum EngineType { WEB, IMAGE };
        public EngineType Type { get; set; }
        public string Name { get; set; }
        public string FirstUrl { get; set; }
        public string SecondUrl { get; set; }
        public string SpaceReplace { get; set; }
        public string IconTileName { get; set; }

        public Uri UriCreator(string text)
        {
            string url = text.Replace(" ", SpaceReplace);
            if (url.Length > 0 && (url[url.Length - 1] == ' ' || url[url.Length - 1] == '.'))
                url = url.Remove(url.Length - 1);
            url = FirstUrl + url + SecondUrl;
            return new Uri(url);
        }
    }
}
