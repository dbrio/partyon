﻿using System;
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



namespace PartyOn
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Código de ejemplo para traducir ApplicationBar
            //BuildLocalizedApplicationBar();
        }


       async private void Pivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            

            if(e.Item.Name =="Home")
            {
                (App.Current.Resources["vmHome"] as viewModels.homeV.UserHomeViewModel).GetUserHomeCommand.Execute(null);

                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.Default;
                Geoposition myLocation = await geolocator.GetGeopositionAsync();
                var latitude = myLocation.Coordinate.Latitude;
                var longitude = myLocation.Coordinate.Longitude;

                MessageBox.Show(longitude.ToString());
            }
            if (e.Item.Name == "Activity")
            {
                (App.Current.Resources["vmUserActivity"] as viewModels.UserActivityViewModel).GetUserActivityCommand.Execute(null);
            } 
           
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