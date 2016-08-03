using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace FastSearcher.Classes.Manager
{
    class FileManager
    {
        static private FileManager instance;
        static public FileManager Instance { get { if (instance == null) instance = new FileManager(); return instance; } }

        FileManager()
        {

        }

        /// <summary>
        /// Metodo che carica il file specificato
        /// </summary>
        /// <param name="path">Percorso del file</param>
        /// <returns>Lista di stringhe contenenti le linee del file</returns>
        public async Task<List<string>> LoadFile(string path)
        {
            List<string> lines = new List<string>();

            if (File.Exists(path))
            {
                Windows.Storage.StorageFile sFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);

                string text = await Windows.Storage.FileIO.ReadTextAsync(sFile);
                string[] ls = text.Split('\n');
                lines = new List<string>(ls);
            }
            else
                throw new FileNotFoundException();
            
            return lines;
        }
    }
}
