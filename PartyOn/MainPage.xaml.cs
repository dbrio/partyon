using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PartyOn.Resources;
using PartyOn.viewModels;
using PartyOn.model;
using Windows.Devices.Geolocation;
using PartyOn.model.userData;
using System.IO;



namespace PartyOn
{
    public partial class MainPage : PhoneApplicationPage
    {
        public int uid;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            
            // Código de ejemplo para traducir ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            GetLocation();

        }


        async public void GetLocation()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.Default;
            Geoposition myLocation = await geolocator.GetGeopositionAsync();
            PlaceLat = myLocation.Coordinate.Latitude;
            PlaceLong = myLocation.Coordinate.Longitude;

            (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).lati = PlaceLat;
            (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).longi = placeLong;
            (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).GetUserHomeCommand.Execute(null);
            
            //MessageBox.Show(string.Format("{0} > {1}", placeLat, placeLong), "geolocalizacion", MessageBoxButton.OK);
        }

        double placeLat;

        public double PlaceLat
        {
            get { return placeLat; }
            set { placeLat = value; }
        }
        double placeLong;

        public double PlaceLong
        {
            get { return placeLong; }
            set { placeLong = value; }
        }
        
       private void Pivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
       
            if(e.Item.Name =="Home")
            {
                (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).lati = PlaceLat;
                (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).longi = placeLong;
                (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).GetUserHomeCommand.Execute(null);
            }
            if (e.Item.Name == "Activity")
            {
                (App.Current.Resources["vmUserActivity"] as viewModels.UserActivityViewModel).GetUserActivityCommand.Execute(null);
            } 
           if (e.Item.Name == "HeyDj")
           {
               (App.Current.Resources["vmSong"] as viewModels.UserSongViewModel).lati = placeLat;
               (App.Current.Resources["vmSong"] as viewModels.UserSongViewModel).longi = placeLong;
               (App.Current.Resources["vmSong"] as viewModels.UserSongViewModel).GetUserSongCommand.Execute(null);

           }
           if (e.Item.Name == "Profile")
           {

               (App.Current.Resources["vmProfile"] as viewModels.UserProfileViewModel).id = uid;
               (App.Current.Resources["vmProfile"] as viewModels.UserProfileViewModel).GetUserProfileCommand.Execute(null);
               (App.Current.Resources["vmPerfil"] as viewModels.PerfilViewModel).id = uid;
               (App.Current.Resources["vmPerfil"] as viewModels.PerfilViewModel).GetUserPerfilCommand.Execute(null);
               
           }
           
        }

       private void addPost(object sender, EventArgs e)
       {
           //(App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).lati = PlaceLat;
           //(App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).longi = PlaceLong;
           //(App.Current.Resources["vmPlace"] as viewModels.UserPlaceViewModel).GetUserPlaceCommand.Execute(null);
           string uri = string.Format("/place.xaml?PlaceLat={0}&PlaceLong={1}&uid={2}&padre=addPost", PlaceLat, PlaceLong, uid);

           NavigationService.Navigate(new Uri(uri, UriKind.Relative));
       }

       private void ApplicationBarIconButton_Click(object sender, EventArgs e)
       {
           string uri = string.Format("/place.xaml?PlaceLat={0}&PlaceLong={1}&uid={2}&padre=addSong", PlaceLat, PlaceLong, uid);

           NavigationService.Navigate(new Uri(uri, UriKind.Relative));
       }

       protected override void OnNavigatedTo(NavigationEventArgs e)
       {
           if (NavigationContext.QueryString.ContainsKey("uid"))
           {
               uid = Convert.ToInt16(NavigationContext.QueryString["uid"]);
           }
           else
           {
               MessageBox.Show("Aun no hay registro ");
           }


       }

       private void listHome_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {
           if(listHome.SelectedIndex != -1)
           {
               string nPlace = (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).UserHomeList.ElementAtOrDefault(listHome.SelectedIndex).PlaceName;
               string latitud = (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).UserHomeList.ElementAtOrDefault(listHome.SelectedIndex).PlaceLat;
               string longitud = (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).UserHomeList.ElementAtOrDefault(listHome.SelectedIndex).PlaceLong;

               string uri = string.Format("/placeDirection.xaml?PlaceLat={0}&PlaceLong={1}&myPlace={2}&myLat={3}&myLongi={4}", latitud, longitud, nPlace, PlaceLat, placeLong);

               NavigationService.Navigate(new Uri(uri, UriKind.Relative));
           }
           
       }

       private void btnLogout_Click(object sender, EventArgs e)
       {
           MessageBoxResult resp = MessageBox.Show("Are you sure you want to log out?", "PartyOn", MessageBoxButton.OKCancel);

           if (resp == MessageBoxResult.OK)
           {
               NavigationService.Navigate(new Uri("/login.xaml?logout=true", UriKind.Relative));
           }

           //if (db.DatabaseExists())
           //{

           //    db.DeleteDatabase();
           //    NavigationService.Navigate(new Uri("/login.xaml?logout=true", UriKind.Relative));
           //}

       }

       private void btnEditaProfile_Click(object sender, EventArgs e)
       {
           NavigationService.Navigate(new Uri("/content/editProfile.xaml", UriKind.Relative));
       }

       private void btnPrivacy_Click(object sender, EventArgs e)
       {
           NavigationService.Navigate(new Uri("/content/privacy.xaml", UriKind.Relative));

       }

       private void btnTerm_Click(object sender, EventArgs e)
       {
           NavigationService.Navigate(new Uri("/content/terms.xaml", UriKind.Relative));

       }
        
        // Código de ejemplo para compilar una ApplicationBar traducida
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Establecer ApplicationBar de la página en una nueva instancia de ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crear un nuevo botón y establecer el valor de texto en la cadena traducida de AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crear un nuevo elemento de menú con la cadena traducida de AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}