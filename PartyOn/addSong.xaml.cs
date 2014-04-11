using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using System.Diagnostics;

namespace PartyOn
{
    public partial class Page1 : PhoneApplicationPage
    {
        double Lat, Longi;
        int uid, PlaceID;
        string PlaceName;

        public Page1()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("PlaceName") && NavigationContext.QueryString.ContainsKey("PlaceID") && NavigationContext.QueryString.ContainsKey("uid") && NavigationContext.QueryString.ContainsKey("Latitud") && NavigationContext.QueryString.ContainsKey("Longitud"))
            {
                Lat = Convert.ToDouble(NavigationContext.QueryString["Latitud"]);
                Longi = Convert.ToDouble(NavigationContext.QueryString["Longitud"]);
                uid = Convert.ToInt16(NavigationContext.QueryString["uid"]);
                PlaceName = Convert.ToString(NavigationContext.QueryString["PlaceName"]);
                PlaceID = Convert.ToInt16(NavigationContext.QueryString["PlaceID"]);
            }
            else
            {
                MessageBox.Show("No GeoData.");
            }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            if (txtSongName.Text != "")
            {
                pbAddSong.Visibility = System.Windows.Visibility.Visible;
                this.Focus();
                AddSong();
            }
            else
            {
                MessageBox.Show("Write the name of the song.", "PartyOn", MessageBoxButton.OK);
            }
            
        }

        private void AddSong()
        {
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var uri = new Uri("http://www.partyonapp.com/API/songpost/", UriKind.Absolute);
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "SongPost_PlaceID", PlaceID);
            postData.AppendFormat("&{0}={1}", "SongPostName", txtSongName.Text);
            postData.AppendFormat("&{0}={1}", "SongPost_User", uid);
            postData.AppendFormat("&{0}={1}", "SongPostLat", Lat);
            postData.AppendFormat("&{0}={1}", "SongPostLong", Longi);
            postData.AppendFormat("&{0}={1}", "SongPostQuote", txtSongQuote.Text);

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
            Debug.WriteLine("Song post added successfully.");
            MessageBox.Show("Song post added successfully.", "PartyOn", MessageBoxButton.OK);

            pbAddSong.Visibility = System.Windows.Visibility.Collapsed;

            string uri = string.Format("/MainPage.xaml?uid={0}", uid);

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

    }
}