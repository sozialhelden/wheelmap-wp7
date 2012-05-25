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
using System.IO.IsolatedStorage;
using System.Device.Location;

namespace WMWP_Map.Settings {
   public class IsoManager {

      #region singleton
      protected static IsoManager instance;
      protected IsoManager() {
         MapSettings = new SettingsMap() { ZoomLevel = 10d, Coordinate = new GeoCoordinate(0, 0) };
      }
      public static IsoManager Instance {
         get {
            if (IsoManager.instance == null)
               IsoManager.instance = new IsoManager();

            return IsoManager.instance;
         }
      }
      #endregion

      private static string ID_MAPSETTINGS = "mapsettings";

      public SettingsMap MapSettings { get; set; }

      /// <summary>
      /// loads map-related user settings from isolated storage
      /// </summary>
      public void Load() {
         if (IsolatedStorageSettings.ApplicationSettings.Contains(ID_MAPSETTINGS))
            MapSettings = IsolatedStorageSettings.ApplicationSettings[ID_MAPSETTINGS] as SettingsMap;

      }

      /// <summary>
      /// saves map-related user settings to isolated storage
      /// </summary>
      public void Save() {
         if (MapSettings != null)
            IsolatedStorageSettings.ApplicationSettings[ID_MAPSETTINGS] = MapSettings;
      }

   }

   public class SettingsMap  {
      public double ZoomLevel { get; set; }
      public GeoCoordinate Coordinate { get; set; }
   }


}
