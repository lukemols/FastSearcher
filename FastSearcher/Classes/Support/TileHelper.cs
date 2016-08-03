using FastSearcher.Classes.Entities;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastSearcher.Classes.Support
{
    class TileHelper
    {
        static TileHelper instance;
        public static TileHelper Instance { get { if (instance == null) instance = new TileHelper(); return instance; } }

        private TileHelper() { }

        public void SetTileOnStartWin8(Engine actualEngine, string searchItem)
        {
            string smallTile = "/Images/Tiles/Small/FlipCycleTileSmall" + actualEngine.IconTileName + ".png";
            string mediumTile = "/Images/Tiles/Medium/FlipCycleTileMedium" + actualEngine.IconTileName + ".png";
            string largeTile = "/Images/Tiles/Large/FlipCycleTileWide" + actualEngine.IconTileName + ".png";


            FlipTileData tile = new FlipTileData
            {
                Title = "Fast Searcher",
                BackTitle = searchItem,
                BackContent = actualEngine.Name,
                WideBackContent = actualEngine.Name,
                SmallBackgroundImage = new Uri(smallTile, UriKind.Relative),
                BackgroundImage = new Uri(mediumTile, UriKind.Relative),
                BackBackgroundImage = new Uri("/Images/Tiles/Medium/FlipCycleTileMedium.png", UriKind.Relative),
                WideBackgroundImage = new Uri(largeTile, UriKind.Relative),
                WideBackBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileWide.png", UriKind.Relative)
            };
            Uri tileUri = new Uri("/MainPage.xaml?name=" + actualEngine.Name + "&&enginetype=" + actualEngine.Type.ToString(), UriKind.Relative);
            ShellTile.Create(tileUri, tile, true);
        }
    }
}
