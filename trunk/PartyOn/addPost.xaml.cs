using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace PartyOn
{

    public partial class addPost : PhoneApplicationPage
        
    {
        bool fisttime = false;
        int vPlaceID, uid;
        double Lat, Long;
        private string nombreImagen = "imagen.jpg";
        private CameraCaptureTask camera = null;
        byte[] byteArray;
        bool TomoFoto = false;
        public addPost()
        {
            InitializeComponent();
            
            
            camera = new CameraCaptureTask();
            camera.Completed += new EventHandler<PhotoResult>(camera_Complete);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("PlaceName") && NavigationContext.QueryString.ContainsKey("PlaceID") && NavigationContext.QueryString.ContainsKey("Latitud") && NavigationContext.QueryString.ContainsKey("Longitud"))
            {
                textPlace.Text= NavigationContext.QueryString["PlaceName"];
                vPlaceID = Convert.ToInt16(NavigationContext.QueryString["PlaceID"]);
                Lat = Convert.ToDouble(NavigationContext.QueryString["Latitud"]);
                Long = Convert.ToDouble(NavigationContext.QueryString["Longitud"]);
                uid = Convert.ToInt16(NavigationContext.QueryString["uid"]);
            }
            else
            {
                textPlace.Text = "Tabaco Bar 504";
                vPlaceID = 1;
            }


            if (fisttime == false)
            {
                fisttime = true;
                camera.Show();

            }
        }

        private void camera_Complete(object sender, PhotoResult e)
        {
            int ancho, alto;
            decimal relacion;
            if(e.TaskResult==TaskResult.OK)
            {
                TomoFoto = true;
                try
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(e.ChosenPhoto);
                    pvwPhoto.Source = image;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        WriteableBitmap btmMap = new WriteableBitmap(image);

                        if (image.PixelWidth > image.PixelHeight)
                        {
                            if (image.PixelWidth > 2000)
                            {
                                ancho = 2000;
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
                            if (image.PixelHeight > 2000)
                            {
                                alto = 2000;
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
                            if (image.PixelHeight > 2000)
                            {
                                alto = 2000;
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
                    }
                }
                catch (ArgumentException x)
                {
                    Console.WriteLine(x.Message);
                }
            }
        }

    
       private void cameraclick(object sender, EventArgs e)
        {
            camera.Show();
        }

        private void addPostClick(object sender, EventArgs e)
        {
            if (TomoFoto == true)
            {
                this.Focus();
                pbAddPost.Visibility = System.Windows.Visibility.Visible;
                Dictionary<string, object> data = new Dictionary<string, object>()
                    {
                        //{"PhotoPostDateTime", DateTime.Today},
                        {"PhotoPost_PlaceID",vPlaceID},
                        {"PhotoPost_User",uid},
                        {"PhotoPost_Lat",Lat},
                        {"PhotoPostLong",Long},
                        {"PhotoPostDescription",txtMessage.Text},
                        {"PhotoPostPhoto",byteArray},
                    };
                PostSubmitter post = new PostSubmitter() { url = "http://partyonapp.com/API/savephotopost/", parameters = data };
                post.Submit();
            }
            else
            {
                MessageBox.Show("You should take a picture.", "PartyOn", MessageBoxButton.OK);
            }
        }
    }

    public class PostSubmitter
    {
        public Dictionary<string, object> parameters { get; set; }
        string boundary = "----------" + DateTime.Now.Ticks.ToString();
        public string url { get; set; }

        public PostSubmitter() { }

        public void Submit()
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            myRequest.Method = "POST";

            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            //writeMultipartObject(postStream, parameters);
            StreamWriter writer = new StreamWriter(postStream);
            if (parameters != null)
            {
                foreach (var entry in parameters as Dictionary<string, object>)
                {
                    WriteEntry(writer, entry.Key, entry.Value);
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();

            postStream.Close();


            request.BeginGetResponse(new AsyncCallback(GetREsponseCallback), request);
        }

        private void GetREsponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamResponse);
            string responseString = streamReader.ReadToEnd();
            Console.WriteLine(responseString);
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(responseString, "Respuesta del servidor web.", MessageBoxButton.OK));
            streamResponse.Close();
            streamReader.Close();
            response.Close();
        }

        public void writeMultipartObject(Stream stream, object data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                foreach (var entry in data as Dictionary<string, object>)
                {
                    WriteEntry(writer, entry.Key, entry.Value);
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();
        }
        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(boundary);
                if (value is byte[])
                {
                    byte[] ba = value as byte[];

                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "sentPhoto.jpg");
                    writer.WriteLine(@"Content-Type: application/octet-stream");
                    //writer.WriteLine(@"Content-Type: image / jpeg");
                    writer.WriteLine(@"Content-Length: " + ba.Length);
                    writer.WriteLine();
                    writer.Flush();
                    Stream output = writer.BaseStream;

                    output.Write(ba, 0, ba.Length);
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }

    }
}