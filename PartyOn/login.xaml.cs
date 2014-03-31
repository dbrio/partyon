using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PartyOn.model.userData;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO.IsolatedStorage;
using System.IO;
using System.Collections.ObjectModel;
using PartyOn;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PartyOn
{
    public partial class login : PhoneApplicationPage
    {
        userDataDbDataContext db = new userDataDbDataContext("isostore:/userDataDBs.sdf");
        string usernameW;
        int uidW;

        public login()
        {
            InitializeComponent();

            //CargarDatosUsuarioDB();

            this.Loaded += new RoutedEventHandler(login_Loaded);
        }

        private void login_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatosUsuarioDB();
        }

        private void CargarDatosUsuarioDB()
        {
            if (db.DatabaseExists())
            {
                pbLogin.Visibility = System.Windows.Visibility.Visible;
                try
                {
                    var results = db.userDataDB2.ToList();
                    var resultado2 = new ObservableCollection<userDataDB>(results);

                    foreach (userDataDB uData in resultado2)
                    {
                        uidW = uData.UserID;
                        usernameW = uData.username;
                    }

                    if (uidW > 0)
                    {
                        //MessageBox.Show(string.Format("Bienvenido de nuevo {0}", usernameW), "PartyOn", MessageBoxButton.OK);

                        string uri = string.Format("/MainPage.xaml?uid={0}&username={1}", uidW, usernameW);
                        NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                    }
                }
                catch (Exception ex)
                {
                    //Aún no hay datos de usuario guardados.
                }
            }
        }

        private void HacerLogin()
        {
            Uri url = new Uri(string.Format("http://192.168.1.109:8000/API/login/?username={0}&password={1}", txtUser.Text, txtPassword.Password), UriKind.Absolute);
            WebClient ClienteWeb = new WebClient();
            ClienteWeb.DownloadStringCompleted += ClienteWeb_DownloadStringCompleted;
            ClienteWeb.DownloadStringAsync(url);
        }

        private void ClienteWeb_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null & !e.Cancelled)
            {
                try
                {
                    var doc = JObject.Parse(e.Result);
                    var query = from item in (JArray)doc["data"]
                                select new userDataDB
                                {
                                    UserID = item["UIdPOChHnApi"].Value<int>(),
                                    username = item["username"].Value<string>(),
                                };
                    var results = query.ToList();

                    usernameW = results.ElementAtOrDefault(0).username;
                    uidW = results.ElementAtOrDefault(0).UserID;

                    userDataDB loginCorrecto = new userDataDB();
                    loginCorrecto.UserID = uidW;
                    loginCorrecto.username = usernameW;

                    if (uidW != 0 && usernameW != "error_No_Login_POChHn")
                    {
                        if (!db.DatabaseExists())
                        {
                            db.CreateDatabase();
                        }

                        db.userDataDB2.InsertOnSubmit(loginCorrecto);
                        db.SubmitChanges();

                        //MessageBox.Show(string.Format("Bienvenido {0}", usernameW), "PartyOn", MessageBoxButton.OK);

                        string uri = string.Format("/MainPage.xaml?uid={0}&username={1}", uidW, usernameW);
                        NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                    }
                    else
                    {
                        MessageBox.Show("Username or password incorrect, please try again.", "PartyOn", MessageBoxButton.OK);
                        txtPassword.Password = "";
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    pbLogin.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                MessageBox.Show("Username or password incorrect, please try again.", "PartyOn", MessageBoxButton.OK);
                txtPassword.Password = "";
                pbLogin.Visibility = System.Windows.Visibility.Collapsed;
                btnEntrar.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pbLogin.Visibility = System.Windows.Visibility.Visible;
            btnEntrar.IsEnabled = false;
            HacerLogin();
        }

    }
}