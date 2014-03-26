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
            if(NavigationContext.QueryString.ContainsKey("PlaceLat"))
            {
                var lati = NavigationContext.QueryString["PlaceLat"];

                double Lati = double.Parse(lati);
                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).lati = Lati;
            }
            if (NavigationContext.QueryString.ContainsKey("PlaceLong"))
            {
                var longi = NavigationContext.QueryString["PlaceLong"];
                double Longi = double.Parse(longi);

                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).longi = Longi;
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

            string nPlace = listBoxPlace.Items[listBoxPlace.SelectedIndex].ToString();
            string uri = string.Format("/addPost.xaml?PlaceName={0}", nPlace);
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