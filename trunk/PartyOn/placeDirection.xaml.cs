using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Services;
using System.Device.Location;

namespace PartyOn
{
    public partial class placeDirection : PhoneApplicationPage
    {
        string PlaceName;
        double lat, longi, myLat, myLongi;
        RouteQuery myQuery = null;
        GeocodeQuery myGeocode = null;
        List<System.Device.Location.GeoCoordinate> myCoordinates = new List<System.Device.Location.GeoCoordinate>();

        public placeDirection()
        {
            InitializeComponent();

           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("myPlace") && NavigationContext.QueryString.ContainsKey("PlaceLat") && NavigationContext.QueryString.ContainsKey("PlaceLong"))
            {
                PlaceName = NavigationContext.QueryString["myPlace"];
                myLat = Convert.ToDouble(NavigationContext.QueryString["myLat"]);
                myLongi = Convert.ToDouble(NavigationContext.QueryString["myLongi"]);

                lat = Convert.ToDouble(NavigationContext.QueryString["PlaceLat"]);
                longi = Convert.ToDouble(NavigationContext.QueryString["PlaceLong"]);
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            directionTo.Text = "Directions to " + PlaceName;
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "ff9f3fa5-b169-4641-b951-014d35f1b18d";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "TDJ3taacDooeNQzK3ZTOyg";



            GenerarMap();

            
        }

        void GenerarMap()
        {
            //double lat = 13.3039361;
            //double longi = -87.1818977;

            //double myLat = 13.309102541101671;
            //double myLongi = -87.18679750841795;

            myCoordinates.Add(new System.Device.Location.GeoCoordinate(lat, longi));
            myCoordinates.Add(new System.Device.Location.GeoCoordinate(myLat, myLongi));

            

            //myGeocode = new GeocodeQuery();
            //myGeocode.GeoCoordinate = new System.Device.Location.GeoCoordinate(myLat, myLongi);
            //myGeocode.QueryCompleted += myGeocode_QueryCompleted;
            //myGeocode.QueryAsync();

            myQuery = new RouteQuery();
            myQuery.Waypoints = myCoordinates;
            myQuery.QueryCompleted += myQuery_QueryCompleted;
            myQuery.QueryAsync();
            mapPlace.Center = new System.Device.Location.GeoCoordinate(myLat, myLongi);

           
            MapLayer myLayer = new MapLayer();

            Pushpin myPushpin = new Pushpin();
            MapOverlay myOverlay = new MapOverlay();
            myPushpin.GeoCoordinate = new System.Device.Location.GeoCoordinate(myLat, myLongi);

            myPushpin.Content = "I'm Here";

            myOverlay.Content = myPushpin;
            myOverlay.GeoCoordinate = new System.Device.Location.GeoCoordinate(myLat, myLongi);
            myLayer.Add(myOverlay);

            Pushpin pushpin = new Pushpin();
            MapOverlay overlay = new MapOverlay();
            pushpin.GeoCoordinate = new System.Device.Location.GeoCoordinate(lat, longi);

            pushpin.Content = PlaceName;

            overlay.Content = pushpin;
            overlay.GeoCoordinate = new System.Device.Location.GeoCoordinate(lat, longi);
            myLayer.Add(overlay);

            mapPlace.Layers.Add(myLayer);

            //Animacion del mapa
            mapPlace.SetView(new System.Device.Location.GeoCoordinate()
            {
                Latitude = lat,
                Longitude = longi
            }, 16, Microsoft.Phone.Maps.Controls.MapAnimationKind.Parabolic);


            
        }

        // void myGeocode_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        //{
        //    if (e.Error == null)
        //    {
        //        myQuery = new RouteQuery();
        //        myQuery.Waypoints = myCoordinates;
        //        myQuery.QueryCompleted +=myQuery_QueryCompleted;
        //        myQuery.QueryAsync();
        //        myGeocode.Dispose();
        //    }
        //}



          void myQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
         {
             if (e.Error == null)
             {
                 Route myRoute = e.Result;
                 MapRoute myMapRoute = new MapRoute(myRoute);
                 mapPlace.AddRoute(myMapRoute);
                 myQuery.Dispose();
             }
         }
  

       

    }

}