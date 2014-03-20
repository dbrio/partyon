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
using System.Windows.Media.Imaging;

namespace PartyOn
{
    public partial class place : PhoneApplicationPage
    {
        public place()
        {
            InitializeComponent();

            CreateLocator();
            
            
        }

        private async void CreateLocator()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.MovementThreshold = 10;
            geolocator.DesiredAccuracyInMeters = 5;
            geolocator.PositionChanged += geolocator_PositionChanged;
            await geolocator.GetGeopositionAsync();
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {

                mapsON.Center = new System.Device.Location.GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);


            });
        }
    }
}