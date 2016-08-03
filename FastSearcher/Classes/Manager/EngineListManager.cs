using System;
using System.Collections.Generic;
using System.Text;
using FastSearcher.Classes.Entities;

namespace FastSearcher.Classes.Manager
{
    class EngineListManager
    {
        static private EngineListManager instance;
        static public EngineListManager Instance { get { if (instance == null) instance = new EngineListManager(); return instance; } }

        Dictionary<string, Engine> webEngines;
        public Dictionary<string, Engine> WebEngines { get { return webEngines; } }

        Dictionary<string, Engine> imageEngines;
        public Dictionary<string, Engine> ImageEngines { get { return imageEngines; } }

        EngineListManager()
        {
            LoadEngines();
        }

        async void LoadEngines()
        {
            if (webEngines != null)
                webEngines.Clear();
            if (imageEngines != null)
                imageEngines.Clear();

            webEngines = new Dictionary<string, Engine>();
            imageEngines = new Dictionary<string, Engine>();

            string path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"\Files\engines.txt";

            FileManager fm = FileManager.Instance;

            try
            {
                List<string> lines = await fm.LoadFile(path);
                foreach(string line in lines)
                {
                    Engine.EngineType type;
                    if (line.Contains("#engine"))
                        type = Engine.EngineType.WEB;
                    else if (line.Contains("#image"))
                        type = Engine.EngineType.IMAGE;
                    else
                        continue;
                    string[] parts = line.Split('|');
                    Engine e = new Engine();
                    e.Name = parts[1];
                    e.FirstUrl = parts[2];
                    e.SecondUrl = parts[3];
                    e.SpaceReplace = parts[4];
                    e.IconTileName = parts[5];
                    e.Type = type;
                    if (type == Engine.EngineType.WEB)
                        webEngines.Add(e.Name, e);
                    else
                        imageEngines.Add(e.Name, e);
                }
            }
            catch {}
        }

        public async void StartResearch(Engine engine, string text)
        {
            if (text == "")
                return;
            string url = text;
            Uri uri = engine.UriCreator(url);
            await Windows.System.Launcher.LaunchUriAsync(uri);
            
        }
    }
}
