using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using PartyOn.model;
using System.Text;
using System.Diagnostics;
using Microsoft.Phone.Tasks;

namespace PartyOn
{
    public partial class signUp : PhoneApplicationPage
    {
        bool DisponibleEmailv, DisponibleUsername;
        public signUp()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            //Validar si los campos están llenos.
            if (txtFirstName.Text != "" && txtLastName.Text != "" && txtEmail.Text != "" && txtUserName.Text != "" && txtPassword.Password != "" && txtPassword2.Password != "")
            {
                if(txtPassword.Password == txtPassword2.Password)
                {
                    DisponibleUsuario();
                }
                else
                {
                    MessageBox.Show("Passwords are different", "PartyOn", MessageBoxButton.OK);
                }
               
            }
            else
            {
                MessageBox.Show("You must enter all data before proceeding", "PartyOn", MessageBoxButton.OK);
            }
        }

        private void ValidarUsuarioEmail()
        {
            if (DisponibleUsername == true)
            {
                if (DisponibleEmailv == true)
                {
                    //Guardar el nuevo usuario.
                    AddNewUser();
                }
                else
                {
                    MessageBox.Show("Error, this email is already registered in PartyOn.", "PartyOn", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Error, this username is already in PartyOn.", "PartyOn", MessageBoxButton.OK);
            }
        }

        private void DisponibleUsuario()
        {
            string uriJson = "http://partyonapp.com/API/comprobarusername/";
            string uri = uriJson + "?uname=" + txtUserName.Text;

            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    var result = a.Result;

                    var doc = JObject.Parse(result);

                    var query = from item in (JArray)doc["data"]
                                select new modelValidator
                                {
                                    Available = item["Available"].Value<string>(),
                                    Error = item["error"].Value<string>(),
                                };
                    var results = query.ToList();

                    if (results.ElementAtOrDefault(0).Available.ToString() == "False")
                    {
                        DisponibleUsername = false;
                    }
                    else
                    {
                        DisponibleUsername = true;
                    }
                }
                else
                {
                    DisponibleUsername = false;
                    MessageBox.Show(a.Error.ToString());
                }
                DisponibleEmail();
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));

        }

        private void DisponibleEmail()
        {
            string uriJson = "http://partyonapp.com/API/comprobaremail/";
            string uri = uriJson + "?uemail=" + txtEmail.Text;

            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    var result = a.Result;

                    var doc = JObject.Parse(result);

                    var query = from item in (JArray)doc["data"]
                                select new modelValidator
                                {
                                    Available = item["Available"].Value<string>(),
                                    Error = item["error"].Value<string>(),
                                };
                    var results = query.ToList();

                    if (results.ElementAtOrDefault(0).Available.ToString() == "False")
                    {
                        DisponibleEmailv = false;
                    }
                    else
                    {
                        DisponibleEmailv = true;
                    }
                }
                ValidarUsuarioEmail();
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            btnEntrar.IsEnabled = true;
            WebBrowserTask webTerms = new WebBrowserTask();
            webTerms.Uri = new Uri("http://www.partyonapp.com/user/password/reset/sendmail/", UriKind.Absolute);
            webTerms.Show();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            btnEntrar.IsEnabled = false;
        }

        private void AddNewUser()
        {
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var uri = new Uri("http://partyonapp.com/API/addnewuser/", UriKind.Absolute);
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "first_name", txtFirstName.Text);
            postData.AppendFormat("&{0}={1}", "last_name", txtLastName.Text);
            postData.AppendFormat("&{0}={1}", "email", txtEmail.Text);
            postData.AppendFormat("&{0}={1}", "username", txtUserName.Text);
            postData.AppendFormat("&{0}={1}", "password", txtPassword.Password);

            webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
            webClient.UploadProgressChanged += webClient_UploadProgressChanged;
            webClient.UploadStringAsync(uri, "POST", postData.ToString());
        }

        void webClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Debug.WriteLine(string.Format("Progress: {0}", e.ProgressPercentage));
        }

        void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Debug.WriteLine("New user added successfully.");
            MessageBox.Show("New user added successfully.", "PartyOn", MessageBoxButton.OK);
            string uri = string.Format("/login.xaml?username={0}&pass={1}", txtUserName.Text, txtPassword.Password);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}