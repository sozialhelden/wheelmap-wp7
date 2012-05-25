using System;

// (c) by Joost van Schaik 
// Source:     http://dotnetbyexample.blogspot.com/2010/10/showing-open-source-maps-on-windows.html
// How to use: http://dotnetbyexample.blogspot.com/2010/10/google-maps-for-windows-phone-7.html
namespace Sozialhelden.Wheelmap.Lib.Global
{
    public class OSMTileSource : Microsoft.Phone.Controls.Maps.TileSource
    {

        public OSMTileSource()
        {
            UriFormat = "http://{0}.tah.openstreetmap.org/Tiles/tile/{1}/{2}/{3}.png";
            _rand = new Random();
        }
        private readonly Random _rand;
        private readonly static string[] TilePathPrefixes = new[] { "a", "b", "c", "d", "e", "f" };
        private string Server
        {
            get
            {
                return TilePathPrefixes[_rand.Next(6)];
            }
        }

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            if (zoomLevel > 0)
            {
                var url = string.Format(UriFormat, Server, zoomLevel, x, y);
                return new Uri(url);
            } return null;
        }

    }
}