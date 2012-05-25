using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;

namespace WMWP_Map {
   public partial class CustomMapControl : UserControl {
      public CustomMapControl() {
         InitializeComponent();

         Loaded += new RoutedEventHandler(CustomMapControl_Loaded);
         map.MapZoom += new EventHandler<MapZoomEventArgs>(map_MapZoom);
      }

      GeoCoordinateWatcher watcher;
      MapLayer imageLayer;

      private void DumpMapSettings() {
         Settings.IsoManager.Instance.MapSettings.ZoomLevel = map.ZoomLevel;
         Settings.IsoManager.Instance.MapSettings.Coordinate = map.TargetCenter;
         Settings.IsoManager.Instance.Save();
      }

      void CustomMapControl_Loaded( object sender, RoutedEventArgs e ) {

         // initialize image overlay layer
         imageLayer = new MapLayer();
         map.Children.Add(imageLayer);

         // setup map control
         try {
            Settings.IsoManager.Instance.Load();
            map.SetView(
               Settings.IsoManager.Instance.MapSettings.Coordinate,
               Settings.IsoManager.Instance.MapSettings.ZoomLevel
               );
         }
         catch (Exception) { }

         // activate geo location watcher
         if (watcher == null) {
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 1.0;
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);

            if (!watcher.TryStart(true, TimeSpan.FromSeconds(5))) {
               MessageBox.Show("Please enable Location Service on the Phone.", "Warning", MessageBoxButton.OK);
            }
         }

      }

      void watcher_StatusChanged( object sender, GeoPositionStatusChangedEventArgs e ) {

      }
      void watcher_PositionChanged( object sender, GeoPositionChangedEventArgs<GeoCoordinate> e ) {
         map.SetView(e.Position.Location, map.ZoomLevel);
         DumpMapSettings();
      }

      void map_MapZoom( object sender, MapZoomEventArgs e ) {
         DumpMapSettings();
      }

      private void Button_Click( object sender, RoutedEventArgs e ) {
         // add a dummy pushbin
         Image pinImage = new Image();
         pinImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Assets/Icons/marker/yes/coffee.png", UriKind.RelativeOrAbsolute));
         pinImage.Width = 37;
         pinImage.Height = 37;
         pinImage.Opacity = 1d;
         pinImage.Stretch = System.Windows.Media.Stretch.None;
         PositionOrigin position = PositionOrigin.BottomCenter;
         imageLayer.AddChild(pinImage, map.TargetCenter, position);
      }




   }
}
