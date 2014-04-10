using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace PartyOn.content
{
    public partial class editProfile : PhoneApplicationPage
    {
        private PhotoChooserTask camera = null;
        bool TomoFoto = false;
        byte[] byteArray;
        int uid;
        string username;

        public editProfile()
        {
            InitializeComponent();
            camera = new PhotoChooserTask();
            camera.Completed += new EventHandler<PhotoResult>(camera_Complete);
        }

        private void btnEditPic_Click(object sender, RoutedEventArgs e)
        {
            pbEditProfile.Visibility = System.Windows.Visibility.Visible;
            camera.Show();  
        }

        private void camera_Complete(object sender, PhotoResult e)
        {
            int ancho, alto;
            decimal relacion;
            if (e.TaskResult == TaskResult.OK)
            {
                TomoFoto = true;
                try
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(e.ChosenPhoto);
                    //pvwPhoto.Source = image;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        WriteableBitmap btmMap = new WriteableBitmap(image);

                        if (image.PixelWidth > image.PixelHeight)
                        {
                            if (image.PixelWidth > 1500)
                            {
                                ancho = 1500;
                                relacion = (Decimal)image.PixelHeight / (Decimal)image.PixelWidth;
                                alto = Convert.ToInt16(image.PixelHeight * relacion);
                            }
                            else
                            {
                                ancho = image.PixelWidth;
                                alto = image.PixelHeight;
                            }
                        }
                        else if (image.PixelWidth < image.PixelHeight)
                        {
                            if (image.PixelHeight > 1500)
                            {
                                alto = 1500;
                                relacion = (Decimal)image.PixelWidth / (Decimal)image.PixelHeight;
                                ancho = Convert.ToInt16(image.PixelWidth * relacion);
                            }
                            else
                            {
                                ancho = image.PixelWidth;
                                alto = image.PixelHeight;
                            }
                        }
                        else
                        {
                            if (image.PixelHeight > 1500)
                            {
                                alto = 1500;
                                relacion = (Decimal)image.PixelWidth / (Decimal)image.PixelHeight;
                                ancho = Convert.ToInt16(image.PixelWidth * relacion);
                            }
                            else
                            {
                                ancho = image.PixelWidth;
                                alto = image.PixelHeight;
                            }
                        }


                        Extensions.SaveJpeg(btmMap, ms, ancho, alto, 0, 80);
                        byteArray = ms.ToArray();

                        actualizarPhoto();
                    }
                }
                catch (ArgumentException x)
                {
                    Console.WriteLine(x.Message);
                }
            }
        }

        private void actualizarPhoto()
        {
            if (TomoFoto == true)
            {
                this.Focus();
                Dictionary<string, object> data = new Dictionary<string, object>()
                    {
                        {"UserProfileID",uid},
                        {"UserProfilePhoto",byteArray},
                    };
                PostSubmitter post = new PostSubmitter() { url = "http://www.partyonapp.com/API/updatephotoprofile/", parameters = data };
                post.Submit();
            }
            else
            {
                MessageBox.Show("You should take a picture.", "PartyOn", MessageBoxButton.OK);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password != "")
            {
                if (txtPassword.Password == txtPassword2.Password)
                {
                    this.Focus();
                    ChangePasswords();
                }
                else
                {
                    MessageBox.Show("Passwords are different.", "PartyOn", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Password don't be null.", "PartyOn", MessageBoxButton.OK);
            }
        }

        private void ChangePasswords()
        {

            string uriJson = "http://www.partyonapp.com/API/changepassword/";

            string uri = uriJson + "?unamePOusertxtChangeHN=" + username + "&psswdPOuserpsswdChangeHNChActPO=" + txtPassword.Password;

            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    var result = a.Result;

                    MessageBox.Show(result.ToString());
                    NavigationService.Navigate(new Uri("/login.xaml?logout=true", UriKind.Relative));
                }
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
       {
           if (NavigationContext.QueryString.ContainsKey("uid"))
           {
               uid = Convert.ToInt16(NavigationContext.QueryString["uid"]);
               username = Convert.ToString(NavigationContext.QueryString["username"]);
           }
           else
           {
               MessageBox.Show("Aun no hay registro ");
           }


       }

    }


}