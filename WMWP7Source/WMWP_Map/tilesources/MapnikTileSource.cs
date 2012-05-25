using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WMWP_Map {
   public class MapnikTileSource : Microsoft.Phone.Controls.Maps.TileSource {
      public MapnikTileSource() {
         UriFormat = "http://{0}.tile.openstreetmap.org/{1}/{2}/{3}.png";
         _rand = new Random();
      }

      private readonly Random _rand;
      private readonly static string[] TilePathPrefixes = new[] { "a", "b", "c", "d", "e", "f" };

      private string Server {
         get {
            return TilePathPrefixes[_rand.Next(6)];
         }
      }

      public override Uri GetUri( int x, int y, int zoomLevel ) {
         if (zoomLevel > 0) {
            var url = string.Format(UriFormat, Server, zoomLevel, x, y);
            return new Uri(url);
         }
         return null;
      }
   }
}
