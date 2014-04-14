using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PartyOn
{
    public partial class multimedia : PhoneApplicationPage
    {
        double latit, longit;
        int uid, idPlace;
        string padre, nPlace;

        public multimedia()
        {
            InitializeComponent();
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            ElegirPhoto("Album");
        }

        private void ElegirPhoto(string tipo)
        {
            string uri = "/login.xaml";

            if (padre == "addPost")
            {
                uri = string.Format("/addPost.xaml?PlaceName={0}&PlaceID={1}&Latitud={2}&Longitud={3}&uid={4}&padre={5}&tipo={6}", nPlace, idPlace, latit, longit, uid, padre, tipo);
            }
            else if (padre == "addSong")
            {
                uri = string.Format("/addSong.xaml?PlaceName={0}&PlaceID={1}&Latitud={2}&Longitud={3}&uid={4}&padre={5}", nPlace, idPlace, latit, longit, uid, padre);
            }

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("Latitud") && NavigationContext.QueryString.ContainsKey("Longitud") && NavigationContext.QueryString.ContainsKey("padre"))
            {
                latit = Convert.ToDouble(NavigationContext.QueryString["Latitud"]);
                longit = Convert.ToDouble(NavigationContext.QueryString["Longitud"]);
                uid = Convert.ToInt16(NavigationContext.QueryString["uid"]);
                padre = Convert.ToString(NavigationContext.QueryString["padre"]);
                nPlace = Convert.ToString(NavigationContext.QueryString["PlaceName"]);
                idPlace = Convert.ToInt16(NavigationContext.QueryString["PlaceID"]);
            }
            else
            {
                MessageBox.Show("No GeoData.");
            }


        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            ElegirPhoto("Camara");
        }
    }
}