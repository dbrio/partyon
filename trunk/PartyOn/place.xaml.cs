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
using PartyOn.viewModels;

namespace PartyOn
{
    public partial class place : PhoneApplicationPage
    {
        double latit, longit;
        int uid;
        public place()
        {
            
            InitializeComponent();

            //CreateLocator();    
          
            
        }

        //private async void CreateLocator()
        //{
        //    Geolocator geolocator = new Geolocator();
        //    geolocator.MovementThreshold = 10;
        //    geolocator.DesiredAccuracyInMeters = 5;
        //    geolocator.PositionChanged += geolocator_PositionChanged;
        //    await geolocator.GetGeopositionAsync();
        //}

        //void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        //{
        //    Dispatcher.BeginInvoke(() =>
        //    {

        //        mapsON.Center = new System.Device.Location.GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);


        //    });
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(NavigationContext.QueryString.ContainsKey("PlaceLat") && NavigationContext.QueryString.ContainsKey("PlaceLong"))
            {
                latit = Convert.ToDouble(NavigationContext.QueryString["PlaceLat"]);
                longit = Convert.ToDouble(NavigationContext.QueryString["PlaceLong"]);
                uid = Convert.ToInt16(NavigationContext.QueryString["uid"]);

                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).lati = latit;
                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).longi = longit;
                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).GetUserPlaceCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("Aun no hay registro ");
            }

             
        }



        //public void geolocation()
        //{
        //    //(App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).lati = Lati;
        //    //(App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).longi = Longi;
        //    (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).GetUserPlaceCommand.Execute(null);
        //}

        private void listBoxPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxPlace.SelectedIndex == -1)
                return;

            string nPlace = (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).UserPlaceList.ElementAtOrDefault(listBoxPlace.SelectedIndex).PlaceName;
            int idPlace = (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).UserPlaceList.ElementAtOrDefault(listBoxPlace.SelectedIndex).PlaceID;
           
            string uri = string.Format("/addPost.xaml?PlaceName={0}&PlaceID={1}&Latitud={2}&Longitud={3}&uid={4}", nPlace, idPlace, latit, longit, uid);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

            listBoxPlace.SelectedIndex = -1;
        }

        //private void listBoxPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string uri = string.Format("addPost.xaml?place={0}", listBoxPlace.ItemsSource);
        //    NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        //}
    }
}