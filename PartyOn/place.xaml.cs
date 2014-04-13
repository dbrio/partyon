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
using System.Text;
using System.Diagnostics;

namespace PartyOn
{
    public partial class place : PhoneApplicationPage
    {
        double latit, longit;
        int uid;
        string padre;
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
            if(NavigationContext.QueryString.ContainsKey("PlaceLat") && NavigationContext.QueryString.ContainsKey("PlaceLong") && NavigationContext.QueryString.ContainsKey("padre"))
            {
                latit = Convert.ToDouble(NavigationContext.QueryString["PlaceLat"]);
                longit = Convert.ToDouble(NavigationContext.QueryString["PlaceLong"]);
                uid = Convert.ToInt16(NavigationContext.QueryString["uid"]);
                padre = Convert.ToString(NavigationContext.QueryString["padre"]);

                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).lati = latit;
                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).longi = longit;
                (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).GetUserPlaceCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("No GeoData.");
            }

             
        }

        private void listBoxPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxPlace.SelectedIndex == -1)
                return;

            string nPlace = (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).UserPlaceList.ElementAtOrDefault(listBoxPlace.SelectedIndex).PlaceName;
            int idPlace = (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).UserPlaceList.ElementAtOrDefault(listBoxPlace.SelectedIndex).PlaceID;

            string uri = "/login.xaml";

            if (padre == "addPost")
            {
                 uri = string.Format("/addPost.xaml?PlaceName={0}&PlaceID={1}&Latitud={2}&Longitud={3}&uid={4}", nPlace, idPlace, latit, longit, uid);
            }
            else if (padre == "addSong")
            {
                 uri = string.Format("/addSong.xaml?PlaceName={0}&PlaceID={1}&Latitud={2}&Longitud={3}&uid={4}", nPlace, idPlace, latit, longit, uid);
            }
            
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

            listBoxPlace.SelectedIndex = -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewPlaceName.Text != "")
            {
                btnAddNewPlace.IsEnabled = false;
                pbPlaces.Visibility = System.Windows.Visibility.Visible;
                AddNewPlace();
            }
            else
            {
                MessageBox.Show("Specify the name of the new place in the text box.", "PartyOn", MessageBoxButton.OK);
            }
        }

        private void AddNewPlace()
        {
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var uri = new Uri("http://partyonapp.com/API/addnewplace/", UriKind.Absolute);
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "PlaceName", txtNewPlaceName.Text);
            postData.AppendFormat("&{0}={1}", "PlaceLat", latit);
            postData.AppendFormat("&{0}={1}", "PlaceLong", longit);

            webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
            webClient.UploadProgressChanged += webClient_UploadProgressChanged;
            webClient.UploadStringAsync(uri, "POST", postData.ToString());
        }

        void webClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Debug.WriteLine(string.Format("Progress: {0}", e.ProgressPercentage));
        }

        private void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Debug.WriteLine("New place added successfully.");
            MessageBox.Show("New place added successfully.", "PartyOn", MessageBoxButton.OK);
            pbPlaces.Visibility = System.Windows.Visibility.Collapsed;
            (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).lati = latit;
            (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).longi = longit;
            (App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).GetUserPlaceCommand.Execute(null);
        }

    }
}